﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Abstractions;
using CQRS.Abstractions.Models;
using CQRS.Implementation.Models;
using CQRS.Models;
using Microsoft.EntityFrameworkCore;

namespace CQRS.Implementation.Handlers.CommandHandlers
{
    public abstract class EntityCommandHandler<TIn, TOut, TEntity> : ICommandHandler<TIn, Task<Result<TOut>>>
        where TIn : ICommand<Task<Result<TOut>>>
        where TEntity : class, IEntity
    {

        protected readonly DbContext DbContext;
        protected readonly DbSet<TEntity> DbSet;
        protected IQueryable<TEntity> Query;
        protected IEnumerable<IAccessFilter<TEntity>> AccessFilters;

        private void ApplyAccessFilters()
        {
            foreach (var accessFilter in AccessFilters)
            {
                Query = accessFilter.Apply(Query);
            }
        }

        protected EntityCommandHandler(DbContext dbContext, IEnumerable<IAccessFilter<TEntity>> accessFilters)
        {
            DbContext = dbContext;
            AccessFilters = accessFilters;
            DbSet = dbContext.Set<TEntity>();
            Query = DbSet;
            ApplyAccessFilters();
        }

        protected virtual Task OnBeforeAction(TEntity entity, TIn input)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnAfterAction(TEntity entity, TIn input)
        {
            return Task.CompletedTask;
        }

        public abstract Task<Result<TOut>> Handle(TIn input);
    }
}
