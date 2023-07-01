using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Domain.DTO.UserManager
{
    public class CustomClaimListDto
    {
        public string DisplayType { get; set; }
        public string ClaimType { get; set; }
        public string DisplayValue { get; set; }
        public string ClaimValue { get; set; }
    }
}
