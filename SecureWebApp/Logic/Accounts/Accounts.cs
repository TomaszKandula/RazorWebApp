using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SecureWebApp.Models.Json;
using SecureWebApp.Models.Database;
using SecureWebApp.Extensions.BCrypt;

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
        /// Perform sign-up action for given PayLoad and Password Salt (reccommended value is > 10).
        /// </summary>
        /// <param name="PayLoad"></param>
        /// <param name="PasswordSalt"></param>
        /// <returns></returns>
        public async Task<int> SignUp(UserCreate PayLoad, int PasswordSalt) 
        { 
        
            var NewUser = new Users()
            { 
                FirstName   = PayLoad.FirstName,
                LastName    = PayLoad.LastName,
                NickName    = PayLoad.NickName,
                EmailAddr   = PayLoad.EmailAddress,
                Password    = BCrypt.HashPassword(PayLoad.Password, BCrypt.GenerateSalt(PasswordSalt)),
                PhoneNum    = null,
                CreatedAt   = DateTime.Now,
                IsActivated = false,
                CountryId   = PayLoad.CountryId,
                CityId      = PayLoad.CityId
            };

            FMainDbContext.Users.Add(NewUser);
            await FMainDbContext.SaveChangesAsync();

            return NewUser.Id;

        }

        /// <summary>
        /// Perform sign-in action and log it to the history table.
        /// </summary>
        /// <param name="EmailAddr"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public async Task<Tuple<Guid, bool>> SignIn(string EmailAddr, string Password)
        {

            var Users = (await FMainDbContext.Users.Where(r => r.EmailAddr == EmailAddr).ToListAsync()).Single();
            if (!Users.IsActivated)
            {
                return Tuple.Create(Guid.Empty, false);
            }

            var CheckPassword = BCrypt.CheckPassword(Password, Users.Password);
            if (!CheckPassword)
            {
                return Tuple.Create(Guid.Empty, true);
            }

            var SessionId = Guid.NewGuid();
            var LogHistory = new SigninHistory()
            {
                UserId = Users.Id,
                LoggedAt = DateTime.Now,
            };

            FMainDbContext.SigninHistory.Add(LogHistory);
            await FMainDbContext.SaveChangesAsync();

            return Tuple.Create(SessionId, true);

        }


    }

}
