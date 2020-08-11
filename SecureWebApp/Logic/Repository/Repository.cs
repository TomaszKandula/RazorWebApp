using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SecureWebApp.Models.Views;
using SecureWebApp.Models.Database;

namespace SecureWebApp.Logic
{

    public class Repository : IRepository
    {

        private readonly MainDbContext FMainDbContext;

        public Repository(MainDbContext AMainDbContext) 
        {
            FMainDbContext = AMainDbContext;
        }

        /// <summary>
        /// Return list of all available countries.
        /// </summary>
        /// <returns></returns>
        public async Task<List<CountryList>> ReturnCountryList()
        {

            var LCountries = await FMainDbContext.Countries
                .AsNoTracking()
                .Select(R => new CountryList()
                {
                    Id = R.Id,
                    Name = R.CountryName
                })
                .ToListAsync();

            return LCountries;

        }

        /// <summary>
        /// Return list of cities for given Country Id.
        /// </summary>
        /// <param name="AId"></param>
        /// <returns></returns>
        public async Task<List<CityList>> ReturnCityList(int AId)
        {

            var LCities = await FMainDbContext.Cities
                .AsNoTracking()
                .Where(R => R.CountryId == AId)
                .Select(R => new CityList()
                {
                    Id = R.Id,
                    Name = R.CityName
                })
                .ToListAsync();

            return LCities;

        }

    }

}
