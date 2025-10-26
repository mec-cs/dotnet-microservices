﻿using System.Security.Claims;

namespace Core.APP.Services.Authentication
{
    public abstract class AuthServiceBase : ServiceBase
    {
        protected List<Claim> GetClaims(int userId, string userName, string[] userRoleNames)
        {
            var claims = new List<Claim>()
            {
                new Claim("Id", userId.ToString()),
                new Claim(ClaimTypes.Name, userName)
            };
            foreach (var userRoleName in userRoleNames)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRoleName));
            }
            return claims;
        }
    }
}
