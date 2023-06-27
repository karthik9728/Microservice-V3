using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Application.Services.Interface
{
    public interface IEmailService
    {
        public Task EmailVerification(string to, string body);

        public Task ForgetPassword(string to, string body);
    }
}
