/*
 * Author: Shon Vivier
 * File Name: FindItemViewModel.cs
 * Project Name: InventoryManagerUI
 * Creation Date: 03/11/2020
 * Modified Date: 03/23/2020
 * Description: The ViewModel for finding items.
 */

using DevExpress.Mvvm.DataAnnotations;

namespace InventoryManagerUI.ViewModels
{
    /// <summary>
    /// The ViewModel for finding items.
    /// </summary>
    [POCOViewModel]
    public class FindItemViewModel
    {
        /// <summary>
        /// The title of the inventory item that the user is searching for.
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// The platform the inventory item is on that the user is searching for.
        /// </summary>
        public virtual string Platform { get; set; }
    }
}