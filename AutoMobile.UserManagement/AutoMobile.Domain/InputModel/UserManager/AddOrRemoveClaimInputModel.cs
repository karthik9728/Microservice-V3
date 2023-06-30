using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Domain.InputModel.UserManager
{
    public class AddOrRemoveClaimInputModel
    {
        public string UserId { get; set; }
        public IEnumerable<ClaimInputModel> Claims { get; set; }

    }
}
