﻿using ExtCore.Mvc.Infrastructure.Actions;
using FluentValidation.AspNetCore;
using Infrastructure.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using Weaving.Dtos;

namespace Infrastructure.Mvc
{
    public class MvcConfig : IAddMvcAction
    {
        public int Priority => 1000;

        public void Execute(IMvcBuilder builder, IServiceProvider sp)
        {
            builder.AddMvcOptions(c =>
            {
                c.Filters.Add(new TransactionDbFilter(sp));

                c.Filters.Add(new GlobalExceptionFilter());
            });

            builder.AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<ManufactureOrderFormValidator>();
            });
        }
    }
}