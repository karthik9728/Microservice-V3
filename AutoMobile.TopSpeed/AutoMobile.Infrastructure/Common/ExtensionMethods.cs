using AutoMobile.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Infrastructure.Common
{
    public static class ExtensionMethods
    {
        public static void SaveCommonFields(this ApplicationDbContext applicationDbContext, IPrincipal principal, IDecodeAccessToken decodeAccessToken)
        {
            //Getting User Id From Token
            var id = decodeAccessToken.UserId();

            Guid userId = id == Guid.Empty ? Guid.Empty : id;

            //Entites going to Insert
            IEnumerable<BaseModel> insertEntites = applicationDbContext.ChangeTracker.Entries()
                .Where(i => i.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                .Select(i => i.Entity)
                .OfType<BaseModel>();

            //Entites going to Update
            IEnumerable<BaseModel> udpateEntites = applicationDbContext.ChangeTracker.Entries()
                .Where(i => i.State == Microsoft.EntityFrameworkCore.EntityState.Modified)
                .Select(i => i.Entity)
                .OfType<BaseModel>();

            //Set Created Date and Created By
            foreach (var item in insertEntites)
            {
                item.CreatedBy = userId;
                item.CreatedOn = DateTime.UtcNow;
                item.ModifiedOn = DateTime.UtcNow;
            }

            //Set Updated Date and Updated By
            foreach (var item in udpateEntites)
            {
                item.ModifiedBy = userId;
                item.ModifiedOn = DateTime.Now;
            }
        }
    }
}
