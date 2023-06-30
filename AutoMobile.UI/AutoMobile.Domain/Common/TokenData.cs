using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Domain.Common
{
    public class TokenData
    {
        public string Role { get; set; }
        public Dictionary<string, string[]> AdditionalClaims { get; set; }
    }
}
