using TE.BE.City.Domain.Interfaces;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Infra.CrossCutting;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace TE.BE.City.Infra.Data.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly TEBECityContext _context;

        public BaseRepository(TEBECityContext context)
        {
            _context = context;
        }
        
        public async Task<bool> Insert(T obj)
        {
            await _context.Set<T>().AddAsync(obj);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return true;
            else
                return false;
        }

        public async Task<bool> Delete(int id)
        {
            var entityToDelete = _context.Set<T>().FirstOrDefault(e => e.Id == id);
            if (entityToDelete != null)
            {
                _context.Set<T>().Remove(entityToDelete);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return true;
            }
            return false;
        }

        public async Task<bool> Edit(T entity)
        {
            var editedEntity = _context.Set<T>().FirstOrDefault(e => e.Id == entity.Id);
            
            _context.Entry(editedEntity).CurrentValues.SetValues(entity);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return true;
            else
                return false;
        }

        public async Task<IEnumerable<T>> Select()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> SelectWithPagination(int skip, int limit)
        {
            return await _context.Set<T>().Skip(skip).Take(limit).ToListAsync();
        }

        public async Task<int> SelectCount()
        {
            return await _context.Set<T>().CountAsync();
        }

        public async Task<T> SelectById(int id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<T>> Filter(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> FilterWithPagination(Expression<Func<T, bool>> predicate, int skip, int limit)
        {
            return await _context.Set<T>().Where(predicate).Skip(skip).Take(limit).ToListAsync();
        }
    }
}
