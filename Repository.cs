using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Ps.EfCoreRepository.SqlServer
{
    public class Repository : IRepository
    {
        private readonly ILogger<Repository> _logger;

        public Repository(DbContext database, ILogger<Repository> logger)
        {
            Database = database;
            this._logger = logger;
        }

        public DbContext Database { get; }



        #region GetList
        public IQueryable<T> GetList<T>() where T : class
        {
            return Database.Set<T>();
        }

        public IQueryable<T> GetList<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return Database.Set<T>().Where(predicate);
        }

        public async Task<IQueryable<T>> GetListAsync<T>() where T : class
        {
            var result = GetList<T>();
            return await Task.FromResult(result);
        }

        public async Task<IQueryable<T>> GetListAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var result = GetList<T>(predicate);
            return await Task.FromResult(result);
        }
        #endregion

        #region GetSingle
        public T GetSingle<T>(int id) where T : class
        {
            return Database.Set<T>().Find(id);
        }
        public T GetSingle<T>(params object[] compositKey) where T : class
        {
            return Database.Set<T>().Find(compositKey);
        }
        public T GetSingle<T>(string primaryKeyValue) where T : class
        {
            return Database.Set<T>().Find(primaryKeyValue);
        }
        public T GetSingle<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return Database.Set<T>().FirstOrDefault(predicate);
        }


        public async Task<T> GetSingleAsync<T>(int id) where T : class
        {
            return await Database.Set<T>().FindAsync(id);
        }
        public async Task<T> GetSingleAsync<T>(params object[] strCompositKey) where T : class
        {
            return await Database.Set<T>().FindAsync(strCompositKey);
        }
        public async Task<T> GetSingleAsync<T>(string primaryKeyValue) where T : class
        {
            return await Database.Set<T>().FindAsync(primaryKeyValue);
        }
        public async Task<T> GetSingleAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await Database.Set<T>().FirstOrDefaultAsync(predicate);
        }
        #endregion

        #region Create
        public T Create<T>(T entity) where T : class
        {
            Database.Set<T>().Add(entity);
            Database.SaveChanges();
            return entity;
        }

        public List<T> Create<T>(List<T> entityList) where T : class
        {
            Database.Set<T>().AddRange(entityList);
            Database.SaveChanges();
            return entityList;
        }

        public async Task<T> CreateAsync<T>(T entity) where T : class
        {
            await Database.Set<T>().AddAsync(entity);
            await Database.SaveChangesAsync();
            return entity;
        }

        public async Task<List<T>> CreateAsync<T>(List<T> entityList) where T : class
        {
            await Database.Set<T>().AddRangeAsync(entityList);
            await Database.SaveChangesAsync();
            return entityList;
        }
        #endregion

        #region Update
        public void Update<T>(T entity) where T : class
        {
            Database.Entry(entity).State = EntityState.Modified;
            Database.SaveChanges();
        }

        public async Task UpdateAsync<T>(T entity) where T : class
        {
            Database.Entry(entity).State = EntityState.Modified;
            await Database.SaveChangesAsync();
        }
        public void Update<T>(List<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                Database.Entry(entity).State = EntityState.Modified;
            }
            Database.SaveChanges();
        }

        public async Task UpdateAsync<T>(List<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                Database.Entry(entity).State = EntityState.Modified;
            }
            await Database.SaveChangesAsync();
        }

        #endregion

        #region Delete
        public void Delete<T>(int id) where T : class
        {
            T? set = Database.Set<T>().Find(id);
            if (set != null)
            {
                Database.Set<T>().Remove(set);
                Database.SaveChanges();
            }
        }

        public async Task DeleteAsync<T>(int id) where T : class
        {
            T? set = await Database.Set<T>().FindAsync(id);
            if (set != null)
            {
                Database.Set<T>().Remove(set);
                await Database.SaveChangesAsync();
            }
        }

        public void Delete<T>(params int[] ids) where T : class
        {
            foreach(var id in ids)
            {
                T? set = Database.Set<T>().Find(id);
                if (set != null)
                {
                    Database.Set<T>().Remove(set);
                }
            }
            Database.SaveChanges();
        }

        public async Task DeleteAsync<T>(params int[] ids) where T : class
        {
            foreach(int id in ids)
            {
                T? set = await Database.Set<T>().FindAsync(id);
                if (set != null)
                {
                    Database.Set<T>().Remove(set);
                }
            }
            await Database.SaveChangesAsync();
        }

        public void Delete<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var sets = Database.Set<T>().Where(predicate);
            foreach(var set in sets)
            {
                Database.Set<T>().Remove(set);
            }
            Database.SaveChanges();
        }

        public async Task DeleteAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var sets = Database.Set<T>().Where(predicate);
            foreach (var set in sets)
            {
                Database.Set<T>().Remove(set);
            }
            await Database.SaveChangesAsync();
        }
        #endregion
    }
}
