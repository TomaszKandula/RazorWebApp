using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using RazorWebApp.Models;

namespace RazorWebApp.LogicContext.Repository
{
    public interface IRepository
    {
        /// <summary>
        /// Return list of all available countries.
        /// </summary>
        /// <param name="ACancellationToken"></param>
        /// <returns></returns>
        Task<List<CountryList>> ReturnCountryList(CancellationToken ACancellationToken = default);

        /// <summary>
        /// Return list of cities for given Country Id.
        /// </summary>
        /// <param name="AId"></param>
        /// <param name="ACancellationToken"></param>
        /// <returns></returns>
        Task<List<CityList>> ReturnCityList(int AId, CancellationToken ACancellationToken = default);
    }
}
