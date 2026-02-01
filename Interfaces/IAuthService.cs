using CoastalPharmacyCRUD.DTOs;
using CoastalPharmacyCRUD.Models;

namespace CoastalPharmacyCRUD.Interfaces
{
    public interface IAuthService
    {

        Task<SYS_User> Register(UserCreateDto userCreate);

        Task<AuthResponseDto?> Login(string email, string password);

    }
}
