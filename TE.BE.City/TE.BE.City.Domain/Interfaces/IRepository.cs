using TE.BE.City.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace TE.BE.City.Domain.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<bool> Insert(T request);

        Task<bool> Delete(int id);

        Task<bool> Edit(T entity);

        Task<IEnumerable<T>> SelectWithPagination(int skip, int limit);

        Task<IEnumerable<T>> Select();

        Task<int> SelectCount();

        Task<T> SelectById(int id);

        Task<IEnumerable<T>> Filter(Expression<Func<T, bool>> predicate);

        Task<IEnumerable<T>> FilterWithPagination(Expression<Func<T, bool>> predicate, int skip, int limit);
    }
}
