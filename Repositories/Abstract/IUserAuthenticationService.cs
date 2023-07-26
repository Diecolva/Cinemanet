using Cinemanet.Models.DTO;

namespace Cinemanet.Repositories.Abstract
{
    public interface IUserAuthenticationService
    {
        Task<Status> LoginAsync(LoginModel model);
        Task LogoutAsync();
        Task<Status> RegisterAsync(RegistrationModel model);
        Task<Status> ChangePasswordAsync(
            ChangePasswordModel model, string username);
    }
}