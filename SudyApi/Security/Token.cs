using Microsoft.IdentityModel.Tokens;
using SudyApi.Models;
using SudyApi.Properties.Enuns;
using SudyApi.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SudyApi.Security
{
    public class Token
    {
        public static string GenerateToken(UserModel user)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(AppSettings.GetKey(ConfigKeys.JwtKey));
            IEnumerable<System.Security.Claims.Claim> claims = UserModel.GetClaims(user);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static string FormatToken(string token)
        {
            string rightToken = string.Empty;

            string[] charsToRemove = new string[] { "{", "}", "Bearer " };

            foreach (string c in charsToRemove)
            {
                rightToken = token.Replace(c, string.Empty);
            }

            return rightToken;
        }

        public static JwtSecurityToken ReadToken(string token)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SecurityToken jsonToken = handler.ReadToken(token);
            JwtSecurityToken json = jsonToken as JwtSecurityToken;

            return json;
        }

        public static bool ValidateToken(string authToken)
        {
            try
            {
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidIssuer = "Sample",
                    ValidAudience = "Sample",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.GetKey(ConfigKeys.JwtKey)))
                };

                SecurityToken validateToken;
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

                if (handler.CanReadToken(authToken))
                {
                    ClaimsPrincipal user = handler.ValidateToken(authToken, validationParameters, out validateToken);

                    DateTime tokenExpiresAt = validateToken.ValidTo;

                    if (tokenExpiresAt < DateTime.Now)
                        return false;

                    string roles = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;

                    if (roles != "")
                    {
                        return true;
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
