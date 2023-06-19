using AutoMobile.Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Infrastructure.Common
{
    public class DecodeAccessToken : IDecodeAccessToken
    {
        private IHttpContextAccessor _httpContextAccessor;

        public DecodeAccessToken()
        {
            _httpContextAccessor = new HttpContextAccessor();
        }

        public Guid UserId()
        {
            StringValues authTokens = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization];

            var token = authTokens.FirstOrDefault();

            token = token != null ? token.Replace("Bearer ", "") : token;

            if (token != null)
            {
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

                var userId = new Guid(jwtToken.Claims.First(claim => claim.Type == SystemConstant.ClaimConstants.UserId).Value);

                return userId;
            }
            return Guid.Empty;
        }
    }
}
