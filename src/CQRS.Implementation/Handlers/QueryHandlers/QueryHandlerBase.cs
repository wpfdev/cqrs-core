﻿using System.Threading.Tasks;
using CQRS.Abstractions;
using CQRS.Implementation.Queries;
using CQRS.Models;

namespace CQRS.Implementation.Handlers.QueryHandlers
{
    public abstract class QueryHandlerBase<TIn, TOut> : IQueryHandler<TIn, Task<Result<TOut>>>
        where TIn : QueryBase<TOut>
    {
        public abstract Task<Result<TOut>> Handle(TIn input);
    }
}
