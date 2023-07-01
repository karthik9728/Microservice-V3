using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Domain.DTO.UserManager
{
    public class AddOrRemoveClaimDto
    {
        public string UserId { get; set; }
        public IEnumerable<ClaimInputModelDto> Claims { get; set; }
    }
}
