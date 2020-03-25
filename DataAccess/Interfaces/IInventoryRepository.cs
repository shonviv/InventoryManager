/*
 * Author: Shon Vivier
 * File Name: IInventoryRepository.cs
 * Project Name: DataAccess
 * Creation Date: 03/09/2020
 * Modified Date: 03/22/2020
 * Description: Creates a repository pattern that defines the logic required to access specific data sources. This allows for better
 * maintainability and decoupling of different types of data infrastructures. This also allows us to expand the application in the
 * future by allowing access from different types of databases, or allowing for functionality with different
 * types of users (such as bookkeeping, which loads in different types of information regarding manufacturing and
 * retail prices with the same basic functions).
 */

using System.Collections.Generic;
using Entities;
using Entities.Interfaces;

namespace DataAccess.Interfaces
{
    /// <summary>
    /// The repository pattern used for handling access to data sources.
    /// </summary>
    public interface IInventoryRepository
    {
        #region Methods

        /// <summary>
        /// Reads in a text file and creates a list of inventory objects based on the input.
        /// </summary>
        /// <param name="path">The file's location.</param>
        /// <returns>A read-only list of inventory item objects.</returns>
        IReadOnlyList<IInventoryItem> LoadInventory(string path);

        /// <summary>
        /// Reads in a list of inventory item objects and saves them to a specified file in a text format.
        /// </summary>
        /// <param name="path">The file's location.</param>
        /// <param name="items">The list of item objects to be saved.</param>
        /// <returns>A boolean value representing whether or not the save was successful.</returns>
        bool SaveInventory(string path, IReadOnlyList<BaseInventoryItem> items);

        #endregion // Methods
    }
}