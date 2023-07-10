using AutoMobile.Domain.DTO.UserManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Domain.DTO.User
{
    public class ApplicationUserUpdateDto
    {
        public ApplicationUserDto UserDto { get; set; }

        public List<CustomClaimListDto> SystemClaims { get; set; }

        public List<ClaimInputModelDto> UserClaims { get; set; }
    }
}
