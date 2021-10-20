using Microsoft.EntityFrameworkCore;
using OperationResult;
using ShortUrl.Domain.Common;
using ShortUrl.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using ShortUrl.Repository.Persistence;

namespace ShortUrl.Repository.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        protected readonly DbSet<TEntity> Db;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            Db = _context.Set<TEntity>();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Result<TEntity> Insert(TEntity entity)
        {
            if (entity == null)
                return Result.Error<TEntity>(new ArgumentNullException(nameof(entity)));

            Db.Add(entity);

            return Result.Success(entity);
        }

        public Result<TEntity> Update(TEntity entity)
        {
            if (entity == null)
                return Result.Error<TEntity>(new ArgumentNullException(nameof(entity)));

            Db.Attach(entity);

            return Result.Success(entity);
        }

        public Result<bool> Delete(TEntity entity)
        {
            if (entity == null)
                return Result.Error<bool>(new ArgumentNullException(nameof(entity)));

            Db.Remove(entity);

            return Result.Success(true);
        }

        public Result<bool> Delete(string id)
        {
            var (_, value) = Select(id);

            return value != null && Delete(value);
        }

        public Result<TEntity> Select(string id)
        {
            return Db.FirstOrDefault(x => x.Id == id);
        }

        public Result<IEnumerable<TEntity>> Select(int page, int size)
        {
            return new(Db.Skip((page - 1) * size).Take(size).AsEnumerable());
        }

        public Result SaveChange()
        {
            _context.SaveChanges();

            return Result.Success();
        }
    }
}
