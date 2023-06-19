using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Infrastructure.Common
{
    public interface IDecodeAccessToken
    {
        public Guid UserId();
    }
}
