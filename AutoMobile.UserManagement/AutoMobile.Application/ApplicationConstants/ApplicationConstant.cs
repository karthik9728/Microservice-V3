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
            public const string User = "USER";
            public const string PremiumUser = "PREMIUMUSER";
        }

        public class CommonMessage
        {
            public const string SystemError = "Server Error Contact System Admin";
        }
    }


    public static class CustomClaimType
    {
        public static string ManagerType = "MANAGERTYPE";
    }

    public static class CustomClaimValue
    {
        public static string JuniorManager = "JUNIORMANAGER";
        public static string SeniorManager = "SENIORMANAGER";
        public static string AssistantManager = "ASSISTANTMANAGER";
        public static string AssociateProductManager = "ASSOCIATEPRODUCTMANAGER";
    }

    public static class CustomClaimPolicy
    {
        public static string JuniorManagerPolicy = "JuniorManagerPolicy";
        public static string SeniorManagerPolicy = "SeniorManagerPolicy";
        public static string AssistantManagerPolicy = "AssistantManagerPolicy";
        public static string AssociateProductManagerPolicy = "AssociateProductManagerPolicy";
    }
}
