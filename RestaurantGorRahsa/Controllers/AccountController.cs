using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantGorRahsa.DTO;
using RestaurantGorRahsa.Services;

namespace RestaurantGorRahsa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountServices _accountServices;
        public AccountController(IAccountServices accountServices ) { 
        _accountServices = accountServices;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO,CancellationToken cancellationToken)
        {
            try
            {
                // Validate input
                if (registerDTO == null)
                {
                    return BadRequest("Invalid registration data.");
                }

                // Call the service to create the account
                var result = await _accountServices.CreateAccount(registerDTO, cancellationToken);

                // Check the result and respond accordingly
                if (result.Token == null)
                {
                    // Registration failed (either user exists or an error occurred)
                    return BadRequest(new { message = result.Message });
                }

                // Registration successful
                return Ok(new
                {
                    name = result.Name,
                    token = result.Token,
                    message = result.Message
                });
            }
            catch (OperationCanceledException)
            {
                // Handle cancellation specifically
                return StatusCode(StatusCodes.Status408RequestTimeout, "The operation was canceled.");
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO,CancellationToken cancellationToken)
        {
    


            try
            {
                if (loginDTO == null)
                {
                    return BadRequest("Login data cannot be null.");
                }

                // Step 2: Call the AccountServices to handle the login
                var authResponse = await _accountServices.LoginAccount(loginDTO, cancellationToken);

                // Step 3: Check if the login was successful
                if (authResponse.Token == null)
                {
                    return Unauthorized(new { message = authResponse.Message });
                }

                // Step 4: Return the response with the token and user name
                return Ok(new { Name = authResponse.Name, Token = authResponse.Token, Message = authResponse.Message });
            }
            catch (Exception ex)
            {
                // Step 5: Handle any unexpected errors
                return  BadRequest($"Error : {ex.Message}");
            }

        }


    }
}
