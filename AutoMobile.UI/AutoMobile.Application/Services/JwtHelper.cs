using AutoMobile.Application.Services.Interface;
using AutoMobile.Domain.Common;
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
        public TokenData ExtractTokenData(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // Decode the JWT token without validating the signature
                var token = tokenHandler.ReadJwtToken(jwtToken);

                // Access the claims from the token.Claims property
                var roleClaim = token.Claims.FirstOrDefault(x => x.Type == SystemConstants.Role.ToLower());

                //// Extract all additional claims
                //var additionalClaims = token.Claims.Where(x => x.Type != SystemConstants.Role.ToLower())
                //                                   .ToDictionary(c => c.Type, c => c.Value);

                // Extract all additional claims
                var additionalClaims = token.Claims.Where(x => x.Type != SystemConstants.Role.ToLower() 
                                                    && x.Type != "exp" && x.Type != "iss" && x.Type != "aud")
                                                   .GroupBy(c => c.Type)
                                                   .ToDictionary(g => g.Key, g => g.Select(c => c.Value).ToArray());

                if (roleClaim != null)
                {
                    // Extract the value of the "role" claim
                    var role = roleClaim.Value;

                    // Return a custom object containing both the role and additional claims
                    return new TokenData
                    {
                        Role = role,
                        AdditionalClaims = additionalClaims
                    };
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
