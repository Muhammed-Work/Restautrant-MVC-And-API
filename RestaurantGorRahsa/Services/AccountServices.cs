using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using RestaurantGorRahsa.DTO;
using RestaurantGorRahsa.Models;

namespace RestaurantGorRahsa.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly RMScontext _contex;
        private readonly TokenService _tokenService;

        public AccountServices(TokenService tokenService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, RMScontext contex)
        {
            _userManager = userManager;
            this.roleManager = roleManager;
            _contex = contex;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> CreateAccount(RegisterDTO registerDTO, CancellationToken cancellationToken)
        {
            try
            {
                // Check if username is already registered
                var isUserNameExist = await _userManager.FindByNameAsync(registerDTO.UserName);
                if (isUserNameExist != null)
                {
                    return new AuthResponseDto(null, null, $"Username {registerDTO.UserName} already exists.");
                }

                // Create a new ApplicationUser instance
                var user = new ApplicationUser
                {
                    Name = registerDTO.Name,
                    UserName = registerDTO.UserName,
                    Email=registerDTO.UserName+"@gmail.com"
                };

                // Attempt to create the user
                var result = await _userManager.CreateAsync(user, registerDTO.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return new AuthResponseDto(null, null, $"Registration failed: {errors}");
                }

                // Assign the user to the specified role
                var roleResult = await _userManager.AddToRoleAsync(user, registerDTO.Role);
                if (!roleResult.Succeeded)
                {
                    var roleErrors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    return new AuthResponseDto(null, null, $"Failed to assign role: {roleErrors}");
                }

                // Generate a JWT token (assuming a method GenerateJwtToken exists)
                var token = _tokenService.GenerateJWtToken(user, registerDTO.Role);

                return new AuthResponseDto(user.Name, token, "Registration successful.");
            }
            catch (OperationCanceledException)
            {
                return new AuthResponseDto(null, null, "The operation was canceled.");
            }
            catch (Exception ex)
            {
                return new AuthResponseDto(null, null, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<AuthResponseDto> LoginAccount(LoginDTO loginDTO, CancellationToken cancellationToken)
        {

            try
            {
                // Check if the user exists by username
                var user = await _userManager.FindByNameAsync(loginDTO.UserName);
                if (user == null)
                {
                    return new AuthResponseDto(null, null, "Invalid username or password.");
                }
                // Check if the password is correct
                var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
                if (!isPasswordValid)
                {
                    return new AuthResponseDto(null, null, "Invalid username or password.");
                }

                // Get roles assigned to the user
                var roles = await _userManager.GetRolesAsync(user);

                // You can either choose to use the first role (if there are multiple roles) or check specific roles
                var userRole = roles.FirstOrDefault();  // Take the first role, or you can modify this to match specific roles

                if (userRole == null)
                {
                    return new AuthResponseDto(null, null, "User has no roles assigned.");
                }

                // Generate a JWT token (assuming a method GenerateJwtToken exists)

                var token = _tokenService.GenerateJWtToken(user, userRole);

                return new AuthResponseDto(user.Name, token, "Login successful.");
            }
            catch(Exception ex)
            {
                return new AuthResponseDto(null, null,$"Error Message : {ex.Message}");
            }

        }
    }
}
