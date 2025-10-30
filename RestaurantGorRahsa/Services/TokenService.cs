using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RestaurantGorRahsa.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestaurantGorRahsa.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static TokenValidationParameters GetTokenValidationParameter(IConfiguration configuration)
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true, // Ensure the issuer is validated
                ValidateAudience = false, // Audience validation is disabled
                ValidateLifetime = true, // Validate token expiration
                ValidateIssuerSigningKey = true, // Ensure the signing key is valid
                ValidIssuer = configuration["Jwt:Issuer"], // The expected issuer
                IssuerSigningKey = GetSymmetricSecurityKey(configuration) // The signing key used for validation
            };
        }

        private string GenerateJwt(IEnumerable<Claim> additionalClaims = null)
        {
            // الحصول على المفتاح الأمني المستخدم لتوقيع التوكن
            var securityKey = GetSymmetricSecurityKey(_configuration);

            // إنشاء بيانات التوقيع باستخدام الخوارزمية HmacSha256
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // تحديد مدة صلاحية التوكن من الإعدادات أو القيمة الافتراضية (60 دقيقة)
            var expireInMinutes = Convert.ToInt32(_configuration["Jwt:ExpireIMinutes"] ?? "60");



            // إنشاء قائمة الادعاءات الافتراضية
            /*
             قوم بإضافة ادعاء (Claim) للتوكن يحتوي على اسم Jti (JSON Token Identifier)، وهو معرف فريد يهدف إلى التحقق من التوكن ومنع تكراره (Replay Attack).
             */
            var lstClaims = new List<Claim> {
            new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            if (additionalClaims?.Any() == true)
            {
                lstClaims.AddRange(additionalClaims);
            }

            var token = new JwtSecurityToken(issuer: _configuration["Jwt:Key"],
                claims: lstClaims,
                expires: DateTime.Now.AddMinutes(expireInMinutes),
                audience: "*", signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);




        }


        public string GenerateJWtToken(ApplicationUser user,string role, IEnumerable<Claim> additionalClainmes = null)
        {
            var lstClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role,role)
            };

            if (additionalClainmes?.Any() == true)
            {
                lstClaims.AddRange(additionalClainmes);
            }
            return GenerateJwt(lstClaims);
        }


        private static SymmetricSecurityKey GetSymmetricSecurityKey(IConfiguration configuration)
        {
            // Retrieve the secret key from configuration
            string key = configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("JWT secret key is not configured.");
            }
            // Convert the key into a byte array using UTF8 encoding
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            return new SymmetricSecurityKey(keyBytes);
        }

    }
}
