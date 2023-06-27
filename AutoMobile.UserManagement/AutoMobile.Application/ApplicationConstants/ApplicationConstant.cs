using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Application.ApplicationConstants
{
    public class ApplicationConstant
    {
        public class CustomRole
        {
            public const string MasterAdmin = "MASTERADMIN";
            public const string SuperAdmin = "SUPERADMIN";
            public const string Admin = "ADMIN";
            public const string Customer = "CUSTOMER";
        }

        public class CommonMessage
        {
            public const string SystemError = "Server Error Contact System Admin";
        }
    }
}
