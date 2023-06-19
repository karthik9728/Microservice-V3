using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Domain.Common
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }

        public bool IsSuccess { get; set; } = false;

        public object Result { get; set; }

        public string DisplayMessage { get; set; } = "";

        public List<ApiError> Errors { get; private set; } = new List<ApiError>();
        public List<ApiWarning> Warnings { get; private set; } = new List<ApiWarning>();

        public void AddWarning(string description)
        {
            ApiWarning apiWarning = new ApiWarning();
            apiWarning.Description = description;
            Warnings.Add(apiWarning);
        }

        public void AddError(string description)
        {
            ApiError apiError = new ApiError();
            apiError.Description = description;
            Errors.Add(apiError);
        }
    }
}
