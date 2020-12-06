using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SecureWebApp.Database;
using SecureWebApp.Database.Models;
using SecureWebApp.Controllers.Models;

namespace SecureWebApp.Logic.Accounts
{

    public class Accounts : IAccounts
    {

        private readonly MainDbContext FMainDbContext;

        public Accounts(MainDbContext AMainDbContext) 
        {
            FMainDbContext = AMainDbContext;
        }

        /// <summary>
        /// Perform sign-up action for given PayLoad and Password Salt (recommended value is > 10).
        /// </summary>
        /// <param name="APayLoad"></param>
        /// <param name="APasswordSalt"></param>
        /// <returns></returns>
        public async Task<int> SignUp(UserCreate APayLoad, int APasswordSalt) 
        { 
        
            var LNewUser = new Users
            { 
                FirstName   = APayLoad.FirstName,
                LastName    = APayLoad.LastName,
                NickName    = APayLoad.NickName,
                EmailAddr   = APayLoad.EmailAddress,
                Password    = BCrypt.BCrypt.HashPassword(APayLoad.Password, BCrypt.BCrypt.GenerateSalt(APasswordSalt)),
                PhoneNum    = null,
                CreatedAt   = DateTime.Now,
                IsActivated = false,
                CountryId   = APayLoad.CountryId,
                CityId      = APayLoad.CityId
            };

            await FMainDbContext.Users.AddAsync(LNewUser);
            await FMainDbContext.SaveChangesAsync();

            return LNewUser.Id;

        }

        /// <summary>
        /// Perform sign-in action and log it to the history table.
        /// </summary>
        /// <param name="AEmailAddr"></param>
        /// <param name="APassword"></param>
        /// <returns></returns>
        public async Task<(Guid SessionId, bool IsSignedIn)> SignIn(string AEmailAddr, string APassword)
        {

            var LUsers = (await FMainDbContext.Users
                .Where(AUsers => AUsers.EmailAddr == AEmailAddr)
                .ToListAsync())
                .Single();
            
            if (!LUsers.IsActivated)
            {
                return (Guid.Empty, false);
            }

            var LCheckPassword = BCrypt.BCrypt.CheckPassword(APassword, LUsers.Password);
            if (!LCheckPassword)
            {
                return (Guid.Empty, true);
            }

            var LSessionId = Guid.NewGuid();
            var LLogHistory = new SigninHistory()
            {
                UserId = LUsers.Id,
                LoggedAt = DateTime.Now,
            };

            await FMainDbContext.SigninHistory.AddAsync(LLogHistory);
            await FMainDbContext.SaveChangesAsync();

            return (LSessionId, true);

        }


    }

}
