using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Application.Services.Interface
{
    public interface IProbationValidator
    {
        Task<bool> ValidateUserProbationAsync(string userId);
    }
}
