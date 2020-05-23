using AQ_PGW.Common.Important;
using AQ_PGW.Core.Interfaces;
using AQ_PGW.Core.Models.DBTables;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IUnitOfWork = AQ_PGW.Infrastructure.Repositories.IUnitOfWork;

namespace AQ_PGW.Infrastructure.Servives
{
    public class LoginServiceRepository : ILoginServiceRepository
    {
        private readonly IConfiguration _configuration;
        private IUnitOfWork _unitOfWork;
        public LoginServiceRepository(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public string Login(string user, string pass)
        {
            var userID = _unitOfWork.Repository<UserAPI>().FirstOrDefault(x => x.UserId == user && x.Password == pass);
            if (userID == null)
            {
                return "";
            }
            var signingKey = Convert.FromBase64String(_configuration["Jwt:SigningSecret"]);
            var expiryDuration = int.Parse(_configuration["Jwt:ExpiryDuration"]);


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = null,              // Not required as no third-party is involved
                Audience = null,            // Not required as no third-party is involved
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(expiryDuration),
                Subject = new ClaimsIdentity(new List<Claim> {
                        new Claim("userid", userID.UserId.ToString())
                    }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var token = jwtTokenHandler.WriteToken(jwtToken);
            return token;
        }
    }
}
