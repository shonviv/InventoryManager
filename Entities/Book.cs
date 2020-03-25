/*
 * Author: Shon Vivier
 * File Name: Book.cs
 * Project Name: Entities
 * Creation Date: 03/09/2020
 * Modified Date: 03/22/2020
 * Description: The book data class that inherits all the functionality of a base inventory item.
 */

namespace Entities
{
    /// <summary>
    /// The book data class. Implements all the functionality of a base inventory item.
    /// </summary>
    public class Book : BaseInventoryItem
    {
        #region Constructor

        /// <summary>
        /// Initializes all the properties of the book and the base inventory item.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="cost"></param>
        /// <param name="genre"></param>
        /// <param name="platform"></param>
        /// <param name="releaseYear"></param>
        /// <param name="author"></param>
        /// <param name="publisher"></param>
        public Book(string title, decimal cost, string genre, string platform, uint releaseYear, string author, string publisher)
            : base(title, cost, genre, platform, releaseYear)
        {
            Author = author;
            Publisher = publisher;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// The author of the book.
        /// </summary>
        public string Author { get; }

        /// <summary>
        /// The publisher of the book.
        /// </summary>
        public string Publisher { get; }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// An override of ToString() that returns all of the book's properties into a file-ready string format.
        /// </summary>
        /// <returns>A string containing all the properties of the book in a file-ready format.</returns>
        public override string ToString()
        {
            return $"Book,{Title},{Cost},{Genre},{Platform},{ReleaseYear},{Author},{Publisher}";
        }

        #endregion // Methods
    }
}