using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using SWII6P2.Models;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace SWII6P2.Services
{
    public static class TokenServices
    {
        private static IConfiguration _configuration;

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = GetKey();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, user.Name)
            }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static byte[] GetKey()
        {
            if (_configuration == null)
            {
                throw new InvalidOperationException("A configuração não foi inicializada. Certifique-se de chamar TokenServices.Initialize antes de usar.");
            }

            return Encoding.ASCII.GetBytes(_configuration["Secret"]);
        }

        public static ClaimsPrincipal? ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = GetKey();

            try
            {
                // Configura os parâmetros de validação
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false, // Se quiser validar o emissor, altere para true
                    ValidateAudience = false, // Se quiser validar a audiência, altere para true
                    ClockSkew = TimeSpan.Zero // Define o tempo de tolerância para expiração
                };

                // Valida o token e obtém as informações de segurança
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                // Retorna as "claims" associadas ao token
                return principal;
            }
            catch (Exception ex)
            {
                // Caso a validação falhe, você pode tratar o erro aqui, logar a exceção, etc.
                Console.WriteLine($"Erro de validação do token: {ex.Message}");
                return null;
            }
        }

        public static async Task<User?> GetTokenUserAsync(ClaimsPrincipal claimsPrincipal, ApplicationDbContext context)
        {
            if (claimsPrincipal == null)
            {
                return null;
            }

            var name = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            return await context.Users.FirstOrDefaultAsync(u => u.Name == name);
        }
    }
}

