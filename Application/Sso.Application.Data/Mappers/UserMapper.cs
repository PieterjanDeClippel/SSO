using System.Threading.Tasks;

namespace Sso.Application.Data.Mappers
{
    internal interface IUserMapper
    {
        Task<Entities.User> Dto2Entity(Dtos.Dtos.User user);
        Task<Dtos.Dtos.User> Entity2Dto(Entities.User user);
    }

    internal class UserMapper : IUserMapper
    {
        public Task<Entities.User> Dto2Entity(Dtos.Dtos.User user)
        {
            if (user == null) return Task.FromResult<Entities.User>(null);

            return Task.FromResult(new Entities.User
            {
                UserName = user.UserName,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                PhoneNumber = user.Phone,
                //TwoFactorEnabled = user.IsTwoFactorEnabled,
                Bypass2faForExternalLogin = user.Bypass2faForExternalLogin,
            });
        }

        public Task<Dtos.Dtos.User> Entity2Dto(Entities.User user)
        {
            if (user == null) return Task.FromResult<Dtos.Dtos.User>(null);

            return Task.FromResult(new Dtos.Dtos.User
            {
                Id = user.Id,
                UserName = user.UserName,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                Phone = user.PhoneNumber,
                IsTwoFactorEnabled = user.TwoFactorEnabled,
                Bypass2faForExternalLogin = user.Bypass2faForExternalLogin,
            });
        }
    }
}
