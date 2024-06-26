﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CountryModel;
using CsvHelper.Configuration;
using System.Globalization;
using weatherserver.Data;
using CsvHelper;
using Microsoft.AspNetCore.Identity;

namespace weatherserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController(CountriesSourceContext db, IHostEnvironment environment, 
        UserManager<WorldCitiesUser> userManager) : ControllerBase
    {
        private readonly string _pathName = Path.Combine(environment.ContentRootPath, "Data/worldcities.csv");

        [HttpPost("User")]
        public async Task<ActionResult> SeedUser()
        {
            (string name, string email) = ("user1", "comp584@csun.edu");
            WorldCitiesUser user = new()
            {
                UserName = name,
                Email = email,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            if (await userManager.FindByNameAsync(name) is not null)
            {
                user.UserName = "user2";
            }
            _ = await userManager.CreateAsync(user, "P@ssw0rd!")
                ?? throw new InvalidOperationException();
            user.EmailConfirmed = true;
            user.LockoutEnabled = false;
            await db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("City")]
        public async Task<ActionResult<City>> SeedCity()
        {
            Dictionary<string, Country> countries = await db.Countries//.AsNoTracking()
            .ToDictionaryAsync(c => c.Name);

            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null
            };
            int cityCount = 0;
            using (StreamReader reader = new(_pathName))
            using (CsvReader csv = new(reader, config))
            {
                IEnumerable<WorldCitiesCsv>? records = csv.GetRecords<WorldCitiesCsv>();
                foreach (WorldCitiesCsv record in records)
                {
                    if (!countries.TryGetValue(record.Country, out Country? value))
                    {
                        Console.WriteLine($"Not found country for {record.City}");
                        return NotFound(record);
                    }

                    if (!record.Population.HasValue || string.IsNullOrEmpty(record.City_ascii))
                    {
                        Console.WriteLine($"Skipping {record.City}");
                        continue;
                    }
                    City city = new()
                    {
                        Name = record.City,
                        Latitude = record.Lat,
                        Longitude = record.Lng,
                        Population = (int)record.Population.Value,
                        CountryId = value.CountryId
                    };
                    db.Cities.Add(city);
                    cityCount++;
                }
                await db.SaveChangesAsync();
            }
            return new JsonResult(cityCount);
        }

        [HttpPost("Country")]
        public async Task<ActionResult<City>> SeedCountry()
        {
            // create a lookup dictionary containing all the countries already existing 
            // into the Database (it will be empty on first run).
            Dictionary<string, Country> countriesByName = db.Countries
                .AsNoTracking().ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);

            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null
            };

            using StreamReader reader = new(_pathName);
            using CsvReader csv = new(reader, config);

            List<WorldCitiesCsv> records = csv.GetRecords<WorldCitiesCsv>().ToList();
            foreach (WorldCitiesCsv record in records)
            {
                if (countriesByName.ContainsKey(record.Country))
                {
                    continue;
                }

                Country country = new()
                {
                    Name = record.Country,
                    Iso2 = record.Iso2,
                    Iso3 = record.Iso3
                };
                await db.Countries.AddAsync(country);
                countriesByName.Add(record.Country, country);
            }

            await db.SaveChangesAsync();

            return new JsonResult(countriesByName.Count);
        }
    }
}
