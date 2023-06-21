using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using static AutoMobile.Domain.ApplicationEnums.ApplicationEnum;

namespace AutoMobile.Domain.Common
{
    public class ApiRequest
    {
        public ApiType ApiType { get; set; } = ApiType.GET;

        public string Url { get; set; } = string.Empty;

        public object Data { get; set; }

        public string Token { get; set; }
    }
}
