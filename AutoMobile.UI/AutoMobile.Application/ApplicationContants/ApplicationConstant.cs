using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Application.ApplicationContants
{
    public class ApplicationConstant
    {
        public class CustomRole
        {
            public const string MasterAdmin = "MASTERADMIN";
            public const string SuperAdmin = "SUPERADMIN";
            public const string Admin = "ADMIN";
            public const string Customer = "CUSTOMER";
            public const string PremiumCustomer = "PREMIUMCUSTOMER";
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
