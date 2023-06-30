using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Domain.Models
{
    public class CustomClaimTypeValue
    {
        public Guid Id { get; set; }
        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }

        public CustomClaimTypeValue(Guid id,string claimType,string claimValue)
        {
            Id = id;
            ClaimType = claimType;
            ClaimValue = claimValue;
        }
    }
}
