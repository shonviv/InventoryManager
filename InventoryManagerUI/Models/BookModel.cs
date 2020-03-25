/*
 * Author: Shon Vivier
 * File Name: BookModel.cs
 * Project Name: InventoryManagerUI
 * Creation Date: 03/11/2020
 * Modified Date: 03/23/2020
 * Description: A book model data class that inherits all the properties of the base inventory model.
 */

using DevExpress.Mvvm.DataAnnotations;

namespace InventoryManagerUI.Models
{
    /// <summary>
    /// A book model data class that inherits all the properties of the base inventory model.
    /// </summary>
    [POCOViewModel]
    public class BookModel : BaseInventoryModel
    {
        #region Properties

        /// <summary>
        /// The author of the book.
        /// </summary>
        public virtual string Author { get; set; }

        /// <summary>
        /// The publisher of the book.
        /// </summary>
        public virtual string Publisher { get; set; }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// An override for GetError from the base inventory model that returns an error relevant to the book model.
        /// </summary>
        /// <returns>A message in the case of an error, or null otherwise</returns>
        protected override string GetError()
        {
            // Check all the book model specific properties for errors.
            if (this[nameof(Author)] != null || this[nameof(Publisher)] != null)
            {
                return "Error";
            }

            // Check all the individual base inventory model properties for errors.
            return base.GetError();
        }

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <param name="columnName">The property with the given name.</param>
        /// <returns>A message in the case of an error, or null otherwise</returns>
        public override string this[string columnName]
        {
            get
            {
                // Switch based on the property of the model.
                switch (columnName)
                {
                    // Return an appropriate error message if a string-type property is null or whitespace.
                    case nameof(Author):
                        if (string.IsNullOrWhiteSpace(Author))
                        {
                            return "Author is required.";
                        }

                        break;

                    case nameof(Publisher):
                        if (string.IsNullOrWhiteSpace(Publisher))
                        {
                            return "Publisher is required.";
                        }

                        break;
                }

                // Check the property in the base class if it is not a book model specific property.
                return base[columnName];
            }
        }

        /// <summary>
        /// Copies all the properties from a source inventory model into this book model.
        /// </summary>
        /// <param name="source"></param>
        public override void CopyFrom(BaseInventoryModel source)
        {
            // Copy the base properties from the source.
            base.CopyFrom(source);

            // Check if the source inventory model is a book model and copy the specific book properties from the bookSource.
            if (source is BookModel bookSource)
            {
                Author = bookSource.Author;
                Publisher = bookSource.Publisher;
            }
        }

        #endregion // Methods
    }
}