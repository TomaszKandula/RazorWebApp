using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SecureWebApp.ViewModel;
using SecureWebApp.Infrastructure.Database;

namespace SecureWebApp.Logic.Repository
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
        public async Task<List<CountryList>> ReturnCountryList(CancellationToken ACancellationToken = default)
        {
            var LCountries = await FMainDbContext.Countries
                .AsNoTracking()
                .Select(ACountries => new CountryList()
                {
                    Id = ACountries.Id,
                    Name = ACountries.CountryName
                })
                .ToListAsync(ACancellationToken);

            return LCountries;
        }

        /// <summary>
        /// Return list of cities for given Country Id.
        /// </summary>
        /// <param name="AId"></param>
        /// <returns></returns>
        public async Task<List<CityList>> ReturnCityList(int AId, CancellationToken ACancellationToken = default)
        {
            var LCities = await FMainDbContext.Cities
                .AsNoTracking()
                .Where(ACities => ACities.CountryId == AId)
                .Select(ACities => new CityList()
                {
                    Id = ACities.Id,
                    Name = ACities.CityName
                })
                .ToListAsync(ACancellationToken);

            return LCities;
        }
    }
}
