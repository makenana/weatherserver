﻿// <auto-generated />
using CountryModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CountryModel.Migrations
{
    [DbContext(typeof(CountriesSourceContext))]
    partial class CountriesSourceContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CountryModel.City", b =>
                {
                    b.Property<int>("CityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CityId"));

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("numeric(18, 4)");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("numeric(18, 4)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Population")
                        .HasColumnType("int");

                    b.HasKey("CityId")
                        .HasName("PK__City__F2D21B76E093FFF1");

                    b.HasIndex("CountryId");

                    b.ToTable("City");
                });

            modelBuilder.Entity("CountryModel.Country", b =>
                {
                    b.Property<int>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CountryId"));

                    b.Property<string>("Iso2")
                        .IsRequired()
                        .HasMaxLength(2)
                        .IsUnicode(false)
                        .HasColumnType("char(2)")
                        .HasColumnName("iso2")
                        .IsFixedLength();

                    b.Property<string>("Iso3")
                        .IsRequired()
                        .HasMaxLength(3)
                        .IsUnicode(false)
                        .HasColumnType("char(3)")
                        .HasColumnName("iso3")
                        .IsFixedLength();

                    b.Property<string>("Name")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.HasKey("CountryId")
                        .HasName("PK__Country__10D1609F0BB69036");

                    b.ToTable("Country");
                });

            modelBuilder.Entity("CountryModel.City", b =>
                {
                    b.HasOne("CountryModel.Country", "Country")
                        .WithMany("Cities")
                        .HasForeignKey("CountryId")
                        .IsRequired()
                        .HasConstraintName("FK_City_Country");

                    b.Navigation("Country");
                });

            modelBuilder.Entity("CountryModel.Country", b =>
                {
                    b.Navigation("Cities");
                });
#pragma warning restore 612, 618
        }
    }
}
