/*
 * Author: Shon Vivier
 * File Name: IInventoryItem.cs
 * Project Name: Entities
 * Creation Date: 03/09/2020
 * Modified Date: 03/22/2020
 * Description: Ensures that all inventory items inheriting this interface provide the same basic properties and functionality.
 */

namespace Entities.Interfaces
{
    /// <summary>
    /// Ensures that all inventory items inheriting this interface provide the same basic properties and functionality.
    /// </summary>
    public interface IInventoryItem
    {
        #region Properties

        // All the properties that each inventory item possesses.
        /// <summary>
        /// The title of the inventory item.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// The cost of then inventory item. Although a decimal's range is significantly lower than a double or a float, it has much more
        /// precision with significant digits (which do have a major effect on manufacturing prices in the long-run).
        /// </summary>
        decimal Cost { get; }

        /// <summary>
        /// The genre of the inventory item.
        /// </summary>
        string Genre { get; }

        /// <summary>
        /// The platform of the inventory item.
        /// </summary>
        string Platform { get; }

        /// <summary>
        /// Since the inventory manager cannot have products that have been released in retail thousands of years ago, we can ignore
        /// negative years (BCE) to double the limit on the maximum number of years (from 2147483647 to 4294967295).
        /// </summary>
        uint ReleaseYear { get; }

        #endregion // Properties
    }
}