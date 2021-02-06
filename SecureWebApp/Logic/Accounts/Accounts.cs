using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SecureWebApp.Shared.Dto;
using SecureWebApp.Infrastructure.Database;
using SecureWebApp.Infrastructure.Domain.Entities;

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
        public async Task<int> SignUp(UserCreateDto APayLoad, int APasswordSalt, CancellationToken ACancellationToken = default) 
        {
            var LHashedPassword = BCrypt.BCrypt
                .HashPassword(APayLoad.Password, BCrypt.BCrypt
                .GenerateSalt(APasswordSalt));
            
            var LNewUser = new Users
            { 
                FirstName   = APayLoad.FirstName,
                LastName    = APayLoad.LastName,
                NickName    = APayLoad.NickName,
                EmailAddr   = APayLoad.EmailAddress,
                Password    = LHashedPassword,
                PhoneNum    = null,
                CreatedAt   = DateTime.Now,
                IsActivated = false,
                CountryId   = APayLoad.CountryId,
                CityId      = APayLoad.CityId
            };

            await FMainDbContext.Users.AddAsync(LNewUser);
            await FMainDbContext.SaveChangesAsync(ACancellationToken);

            return LNewUser.Id;
        }

        /// <summary>
        /// Perform sign-in action and log it to the history table.
        /// </summary>
        /// <param name="AEmailAddr"></param>
        /// <param name="APassword"></param>
        /// <returns></returns>
        public async Task<(Guid SessionId, bool IsSignedIn, bool IsExisting)> SignIn(string AEmailAddr, string APassword, CancellationToken ACancellationToken = default)
        {
            var LUsers = await FMainDbContext.Users
                .Where(AUsers => AUsers.EmailAddr == AEmailAddr)
                .ToListAsync(ACancellationToken);

            if (!LUsers.Any())
            {
                return (Guid.Empty, false, false);
            }

            if (!LUsers.Single().IsActivated) 
            {
                return (Guid.Empty, false, true);
            }

            var LCheckPassword = BCrypt.BCrypt.CheckPassword(APassword, LUsers.Single().Password);
            if (!LCheckPassword)
            {
                return (Guid.Empty, true, true);
            }

            var LSessionId = Guid.NewGuid();
            var LLogHistory = new SigninHistory()
            {
                UserId = LUsers.Single().Id,
                LoggedAt = DateTime.Now,
            };

            await FMainDbContext.SigninHistory.AddAsync(LLogHistory);
            await FMainDbContext.SaveChangesAsync(ACancellationToken);

            return (LSessionId, true, true);
        }
    }
}
