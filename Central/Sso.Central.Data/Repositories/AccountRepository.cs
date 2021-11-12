using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Sso.Central.Data.Repositories
{
    public interface IAccountRepository
    {
        Task<Dtos.User> Register(Dtos.User user, string password);
    }

    internal class AccountRepository : IAccountRepository
    {
        private readonly UserManager<Entities.User> userManager;
        private readonly SignInManager<Entities.User> signinManager;
        public AccountRepository(
            UserManager<Entities.User> userManager,
            SignInManager<Entities.User> signinManager)
        {
            this.userManager = userManager;
            this.signinManager = signinManager;
        }

        public async Task<Dtos.User> Register(Dtos.User user, string password)
        {
            var entity = new Entities.User
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
            };
            await userManager.CreateAsync(entity, password);
            return new Dtos.User
            {
                Id = user.Id,
                Email = entity.Email,
                UserName = entity.UserName,
            };
        }
    }
}
