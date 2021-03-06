﻿using System;
using System.Threading.Tasks;
using CQRS.Abstractions;
using CQRS.Models;
using SimpleInjector;

namespace CQRS.Implementation
{
    public class HandlerDispatcher : IHandlerDispatcher
    {
        private readonly Container _container;

        public HandlerDispatcher(Container container)
        {
            _container = container;
        }

        public async Task<Result<TOut>> Handle<TIn, TOut>(TIn input)
        {
            if (input == null)
            {
                throw new NullReferenceException(typeof(TIn).Name);
            }

            var handler = (IHandler<TIn, Task<Result<TOut>>>)_container.GetInstance(typeof(IHandler<TIn, Task<Result<TOut>>>));

            return await handler.Handle(input);
        }

        public async Task<Result<object>> Handle(Type In, object input)
        {
            if (input == null)
            {
                throw new NullReferenceException(In.Name);
            }

            var handlerType = typeof(IHandler<,>).MakeGenericType(In, typeof(Task<Result<object>>));

            var handler = _container.GetInstance(handlerType);

            var handleMethod = handlerType.GetMethod("Handle");
            
            var resultTask = (Task<Result<object>>)handleMethod.Invoke(handler, new[] {input});

            return await resultTask;
        }
    }
}
