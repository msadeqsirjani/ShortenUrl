using OperationResult;
using ShortUrl.Domain.Common;
using System;
using System.Collections.Generic;

namespace ShortUrl.Domain.Interfaces
{
    public interface IBaseRepository<TEntity> : IDisposable where TEntity : BaseEntity
    {
        Result<TEntity> Insert(TEntity entity);
        Result<TEntity> Update(TEntity entity);
        Result<bool> Delete(TEntity entity);
        Result<bool> Delete(string id);
        Result<TEntity> Select(string id);
        Result<IEnumerable<TEntity>> Select(int page, int size);
        Result SaveChange();
    }
}