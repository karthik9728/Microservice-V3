using AutoMobile.Application.Services.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AutoMobile.Domain.ApplicationConstants.SD;

namespace AutoMobile.Application.Services
{
    public class JwtHelper : IJwtHelper
    {
        public string ExtractRoleFromToken(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // Decode the JWT token without validating the signature
                var token = tokenHandler.ReadJwtToken(jwtToken);

                // Access the claims from the token.Claims property
                var roleClaim = token.Claims.FirstOrDefault(x => x.Type == SystemConstants.Role.ToLower());

                if (roleClaim != null)
                {
                    // Extract the value of the "role" claim
                    return roleClaim.Value;
                }
                else
                {
                    // "role" claim not found
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occurred during decoding
                Console.WriteLine("JWT decoding failed: " + ex.Message);
                return null;
            }
        }
    }
}
