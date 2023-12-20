﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IRepository<TEntity>
    {
        IQueryable<TEntity> Query();
        Task<TEntity> GetByIdAsync(int id);
        Task InsertAsync(TEntity entity);
        void SetForInsert(TEntity entity);
        Task DeleteAsync(int id);
        void SetForUpdate(TEntity entity);
        Task UpdateAsync(TEntity entity);
        IQueryable<TEntity> GetQueryPaged(int currentPage, int recordsPerPage);
    }
}
