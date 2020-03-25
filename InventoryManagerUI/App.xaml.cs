/*
 * Author: Shon Vivier
 * File Name: App.xaml.cs
 * Project Name: InventoryManagerUI
 * Creation Date: 03/12/2020
 * Modified Date: 03/23/2020
 * Description: Interaction logic for App.xaml.
 */

using System;
using System.Windows;

namespace InventoryManagerUI
{
    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Register all services in the DI container.
            ServiceProvider = Bootstrapper.ConfigureServices();
        }
    }
}