using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Application.Services.Interface
{
    public interface IJwtHelper
    {
        string ExtractRoleFromToken(string jwtToken);
    }
}
