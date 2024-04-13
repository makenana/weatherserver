using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CountryModel;
using weatherserver.DTO;
using Microsoft.AspNetCore.Authorization;

namespace weatherserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController(CountriesSourceContext context) : ControllerBase
    {

        // GET: api/Cities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> GetCities()
        {
            return await context.Cities.ToListAsync();
        }

        [Authorize]
        [HttpGet("GetPopulation")]
        public async Task<ActionResult<IEnumerable<CountryPopulation>>> GetPopulation()
        {
            IQueryable<CountryPopulation> x = from c in context.Countries
                     select new CountryPopulation
                     {
                         Name = c.Name,
                         CountryId = c.CountryId,
                         Population = c.Cities.Sum(t => t.Population)
                     };
            return await x.ToListAsync();
        }

        [HttpGet("GetPopulation2")]
        public async Task<ActionResult<IEnumerable<CountryPopulation>>> GetPopulation2()
        {
            IQueryable<CountryPopulation> x = context.Countries.Select(c => 
                                              new CountryPopulation
                                              {
                                                  Name = c.Name,
                                                  CountryId = c.CountryId,
                                                  Population = c.Cities.Sum(t => t.Population)
                                              });
            return await x.ToListAsync();
        }
    }
}