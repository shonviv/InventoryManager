/*
 * Author: Shon Vivier
 * File Name: BaseInventoryModel.cs
 * Project Name: InventoryManagerUI
 * Creation Date: 03/11/2020
 * Modified Date: 03/23/2020
 * Description: A base inventory model that contains all the information relevant to all inventory item models.
 */

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagerUI.Models
{
    /// <summary>
    /// A base inventory model that contains all the information relevant to all inventory item models.
    /// </summary>
    public class BaseInventoryModel : IDataErrorInfo
    {
        #region Properties

        /// <summary>
        /// The title of the inventory item.
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// The cost of the inventory item. The cost is made nullable to avoid setting a default numeric value before the user sets it manually.
        /// </summary>
        public virtual decimal? Cost { get; set; }

        /// <summary>
        /// The genre of the inventory item.
        /// </summary>
        public virtual string Genre { get; set; }

        /// <summary>
        /// The platform the inventory item is on.
        /// </summary>
        public virtual string Platform { get; set; }

        /// <summary>
        /// The year the inventory item was released. The release year is made nullable to avoid setting a default numeric value before the user sets it manually.
        /// </summary>
        public virtual uint? ReleaseYear { get; set; }

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        public string Error => GetError();

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <param name="columnName">The property with the given name.</param>
        /// <returns>A message in the case of an error, or null otherwise</returns>
        public virtual string this[string columnName]
        {
            get
            {
                // Switch based on the property of the model.
                switch (columnName)
                {
                    // Return an appropriate error message if a string-type property is null or whitespace.
                    case nameof(Title):
                        if (string.IsNullOrWhiteSpace(Title))
                        {
                            return "Title is required.";
                        }

                        break;

                    case nameof(Genre):
                        if (string.IsNullOrWhiteSpace(Genre))
                        {
                            return "Genre is required.";
                        }

                        break;

                    case nameof(Platform):
                        if (string.IsNullOrWhiteSpace(Platform))
                        {
                            return "Platform is required.";
                        }

                        break;

                    // Return an appropriate error message if a value-type property is null.
                    case nameof(Cost):
                        if (Cost == null)
                        {
                            return "Cost is required.";
                        }

                        break;

                    case nameof(ReleaseYear):
                        if (ReleaseYear == null)
                        {
                            return "Release year is required.";
                        }

                        break;
                }

                // Return null if there are no errors
                return null;
            }
        }

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <returns>A message in the case of an error, or null otherwise</returns>
        protected virtual string GetError()
        {
            // Check all the individual base inventory model properties for errors.
            // Return null if no properties have any error, otherwise return an error string.
            return this[nameof(Title)] == null && 
                   this[nameof(Cost)] == null && 
                   this[nameof(Genre)] == null &&
                   this[nameof(Platform)] == null &&
                   this[nameof(ReleaseYear)] == null
                ? null : "Error.";
        }

        /// <summary>
        /// Copies all the information from a source base inventory model to this base inventory model.
        /// </summary>
        /// <param name="source">The base inventory model being copied.</param>
        public virtual void CopyFrom(BaseInventoryModel source)
        {
            // Check if the source is null and throw an exception if necessary.
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            // Copy all the property information from the source model.
            Title = source.Title;
            Cost = source.Cost;
            Genre = source.Genre;
            Platform = source.Platform;
            ReleaseYear = source.ReleaseYear;
        }

        #endregion // Methods
    }
}