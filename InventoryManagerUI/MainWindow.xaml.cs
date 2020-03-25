/*
 * Author: Shon Vivier
 * File Name: MainWindow.cs
 * Project Name: InventoryManagerUI
 * Creation Date: 03/09/2020
 * Modified Date: 03/21/2020
 * Description: Interaction logic for MainWindow.xaml.
 */

using System.Windows;
using DevExpress.Xpf.Core;
using InventoryManagerUI.ViewModels;

namespace InventoryManagerUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = App.ServiceProvider.GetService(typeof(MainViewModel));
        }

        /// <summary>
        /// Ensures that at least one tab view is selected at any given time.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnInventoryCheckChanged(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            // Retrieve the button and return if it is checked.
            var button = e.Item as DevExpress.Xpf.Bars.BarCheckItem;
            if (button?.IsChecked != false)
            {
                return;
            }

            // If no buttons are checked, activate one to ensure that at least one is checked.
            if (bBooks.IsChecked == false &&
                bMovies.IsChecked == false &&
                bVideoGames.IsChecked == false)
            {
                button.IsChecked = true;
            }
        }

        /// <summary>
        /// Reset the currently focused item whenever a tab is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectedTabChildChanged(object sender, ValueChangedEventArgs<FrameworkElement> e)
        {
            // If our DataContext is on the MainViewModel as it should be, set the FocusedItemModel to null.
            if (DataContext is MainViewModel viewModel)
            {
                viewModel.FocusedItemModel = null;
            }
        }
    }
}