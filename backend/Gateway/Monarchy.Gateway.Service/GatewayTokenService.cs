using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Monarchy.Gateway.Extensibility.Interface;
using Monarchy.Gateway.Extensibility.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using GatewayConfiguration = Monarchy.Gateway.Extensibility.Configuration;

namespace Monarchy.Gateway.Service
{
    public class GatewayTokenService : ITokenService
    {
        private readonly GatewayConfiguration configuration;
        private readonly byte[] key;

        public GatewayTokenService(IOptions<GatewayConfiguration> options)
        {
            configuration = options.Value;
            key = Encoding.ASCII.GetBytes(configuration.Secret);
        }

        public TokenModel Create(string email, IEnumerable<string> permissions)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("Username cannot be empty");
            }
            var claims = permissions.Select(p => new Claim("permission", p)).ToList();
            claims.Add(new Claim(ClaimTypes.Email, email));
            var handler = new JwtSecurityTokenHandler();
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddSeconds(configuration.TokenExpirySecs),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = handler.CreateToken(descriptor);
            return new TokenModel
            {
                AccessToken = handler.WriteToken(token)
            };
        }

        public string GetEmailFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            return (handler.ReadJwtToken(token) as JwtSecurityToken)
                .Claims
                .SingleOrDefault(c => c.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?
                .Value;
        }
    }
}
