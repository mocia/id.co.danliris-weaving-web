﻿using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Infrastructure.Mvc.Filters
{
    public class TransactionDbFilter : IActionFilter
    {
        private DbContext _dbContext;
        private readonly IServiceProvider _provider;

        private IDbContextTransaction _currentTransaction;

        public TransactionDbFilter(IServiceProvider provider)
        {
            _provider = provider;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (_currentTransaction != null)
            {
                if (context.Exception != null)
                    _currentTransaction.Rollback();
                else
                    _currentTransaction.Commit();

                _currentTransaction.Dispose();
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (new string[] { "POST", "PUT", "DELETE" }.Contains(context.HttpContext.Request.Method))
            {
                //if (_dbContext == null)
                _dbContext = _provider.GetService<IStorageContext>() as DbContext;

                _currentTransaction = _dbContext.Database.BeginTransaction();
            }
        }
    }
}