using AutoMobile.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Application.Contracts.Persistence
{
    public interface IGenericRepository<T> where T : BaseModel
    {
        Task Create(T entity);

        Task Delete(T entity);

        Task<List<T>> Get(Expression<Func<T, bool>> predicate);

        Task<List<T>> GetAllAsync();

        IEnumerable<T> Query(Expression<Func<T, bool>> predicate);

        IEnumerable<T> Query();

        Task<T> GetByIdAsync(int id);

        Task<bool> IsRecordExsits(Expression<Func<T, bool>> predicate);

    }
}
