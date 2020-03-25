/*
 * Author: Shon Vivier
 * File Name: BaseInventoryItem.cs
 * Project Name: Entities
 * Creation Date: 03/09/2020
 * Modified Date: 03/22/2020
 * Description: An abstract base-class containing information that all inventory items contain.
 */

using Entities.Interfaces;

namespace Entities
{
    /// <summary>
    /// An abstract base-class containing information that all inventory items contain.
    /// </summary>
    public abstract class BaseInventoryItem : IInventoryItem
    {
        #region Constructor

        /// <summary>
        /// Initializes all the properties of the base inventory item.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="cost"></param>
        /// <param name="genre"></param>
        /// <param name="platform"></param>
        /// <param name="releaseYear"></param>
        protected BaseInventoryItem(string title, decimal cost, string genre, string platform, uint releaseYear)
        {
            Title = title;
            Cost = cost;
            Genre = genre;
            Platform = platform;
            ReleaseYear = releaseYear;
        }

        #endregion Constructor

        #region Properties

        // All the properties that each inventory item possesses.
        /// <summary>
        /// The title of the inventory item.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// The cost of then inventory item. Although a decimal's range is significantly lower than a double or a float, it has much more
        /// precision with significant digits (which do have a major effect on manufacturing prices in the long-run).
        /// </summary>
        public decimal Cost { get; }

        /// <summary>
        /// The genre of the inventory item.
        /// </summary>
        public string Genre { get; }

        /// <summary>
        /// The platform of the inventory item.
        /// </summary>
        public string Platform { get; }

        /// <summary>
        /// Since the inventory manager cannot have products that have been released in retail thousands of years ago, we can ignore
        /// negative years (BCE) to double the limit on the maximum number of years (from 2147483647 to 4294967295).
        /// </summary>
        public uint ReleaseYear { get; }

        #endregion // Properties
    }
}