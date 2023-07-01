using AutoMobile.Domain.ApplicationConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Domain.Models
{
    public class CustomClaimList
    {
        public string DisplayType { get; set; }
        public string ClaimType { get; set; }
        public string DisplayValue { get; set; }
        public string ClaimValue { get; set; }

        public List<CustomClaimList> GetClaimList()
        {

            List<CustomClaimList> claims = new List<CustomClaimList>
            {
                new CustomClaimList
                {
                    DisplayType = "Manager Type",
                    ClaimType = CustomClaimType.ManagerType,
                    ClaimValue = CustomClaimValue.JuniorManager,
                    DisplayValue = "Junior Manager",
                },
                new CustomClaimList
                {
                    DisplayType = "Manager Type",
                    ClaimType = CustomClaimType.ManagerType,
                    ClaimValue = CustomClaimValue.SeniorManager,
                    DisplayValue = "Senior Manager"
                },
                new CustomClaimList
                {
                    DisplayType = "Manager Type",
                    ClaimType = CustomClaimType.ManagerType,
                    ClaimValue = CustomClaimValue.AssistantManager,
                    DisplayValue = "Assistant Manager"
                },
                new CustomClaimList
                {
                    DisplayType = "Manager Type",
                    ClaimType = CustomClaimType.ManagerType,
                    ClaimValue = CustomClaimValue.AssociateProductManager,
                    DisplayValue = "Associate Product Manager"
                },

            };

            return claims;
        }
    }
}
