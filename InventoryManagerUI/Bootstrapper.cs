/*
 * Author: Shon Vivier
 * File Name: Bootstrapper.cs
 * Project Name: InventoryManagerUI
 * Creation Date: 03/10/2020
 * Modified Date: 03/21/2020
 * Description: A bootstrapper that initializes the inversion of control container and registers all the types necessary for dependency injection (sets up MVVM relationship).
 */

using System;
using System.Linq;
using System.Reflection;
using DataAccess;
using DataAccess.Interfaces;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using InventoryManagerUI.Mappers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace InventoryManagerUI
{
    public static class Bootstrapper
    {
        /// <summary>
        /// A bootstrapper that initializes the inversion of control container and registers all the types necessary for dependency injection (sets up MVVM relationship).
        /// </summary>
        /// <returns>Returns the DI container.</returns>
        public static IServiceProvider ConfigureServices()
        {
            // Registers the logger, the repositories and mapper.
            IServiceCollection services = new ServiceCollection()
                .AddLogging(builder => builder.AddFilter("Microsoft", LogLevel.Warning).AddFilter("System", LogLevel.Warning).AddNLog())
                .AddSingleton<IInventoryRepository, FileInventoryRepository>()
                .AddSingleton<InventoryMapper>();

            // Register all view models in the assembly with the custom attribute [POCOViewModel].
            var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
            foreach (var vmt in assembly.GetTypes().Where(t => Attribute.GetCustomAttribute(t, typeof(POCOViewModelAttribute)) != null))
            {
                services.AddTransient(vmt, ViewModelSource.GetPOCOType(vmt));
            }

            // Create and return the DI container.
            return services.BuildServiceProvider();
        }
    }
}