﻿using System;
using Configuration;
using DataSource;
using DataSource.Entities;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DependencyInjection
{
    public static class HttpContextAccessorServiceExtensions
    {
        public static IServiceCollection AddHttpContextAccessorServiceCollection(this IServiceCollection services)
        {
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>(); 
            return services;
        }
    }
}