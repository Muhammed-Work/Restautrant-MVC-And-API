using RestaurantGorRahsa.DTO;

namespace RestaurantGorRahsa.Services
{
    public interface IAccountServices
    {
        public Task<AuthResponseDto> CreateAccount(RegisterDTO registerDTO, CancellationToken cancellationToken);
        public Task<AuthResponseDto> LoginAccount(LoginDTO loginDTO, CancellationToken cancellationToken);
    }
}
