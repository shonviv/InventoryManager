/*
 * Author: Shon Vivier
 * File Name: MovieModel.cs
 * Project Name: InventoryManagerUI
 * Creation Date: 03/11/2020
 * Modified Date: 03/23/2020
 * Description: A movie model data class that inherits all the properties of the base inventory model.
 */

using DevExpress.Mvvm.DataAnnotations;
using System;

namespace InventoryManagerUI.Models
{
    /// <summary>
    /// A movie model data class that inherits all the properties of the base inventory model.
    /// </summary>
    [POCOViewModel]
    public class MovieModel : BaseInventoryModel
    {
        #region Properties

        /// <summary>
        /// The director of the movie.
        /// </summary>
        public virtual string Director { get; set; }

        /// <summary>
        /// The duration of time of the movie.
        /// </summary>
        public virtual TimeSpan? Duration { get; set; }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// An override for GetError from the base inventory model that returns an error relevant to the movie model.
        /// </summary>
        /// <returns>A message in the case of an error, or null otherwise</returns>
        protected override string GetError()
        {
            // Check all the movie model specific properties for errors.
            if (this[nameof(Director)] != null || this[nameof(Duration)] != null)
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
                    case nameof(Director):
                        if (string.IsNullOrWhiteSpace(Director))
                        {
                            return "Author is required.";
                        }

                        break;
                        
                    // Return an appropriate error message if the duration property is null.
                    case nameof(Duration):
                        if (Duration == null)
                        {
                            return "Duration is required.";
                        }

                        break;
                }

                // Check the property in the base class if it is not a movie model specific property.
                return base[columnName];
            }
        }

        /// <summary>
        /// Copies all the properties from a source inventory model into this movie model.
        /// </summary>
        /// <param name="source"></param>
        public override void CopyFrom(BaseInventoryModel source)
        {
            // Copy the base properties from the source.
            base.CopyFrom(source);

            // Check if the source inventory model is a movie model and copy the specific movie properties from the movieSource.
            if (source is MovieModel movieSource)
            {
                Director = movieSource.Director;
                Duration = movieSource.Duration;
            }
        }

        #endregion // Methods
    }
}