/*
 * Author: Shon Vivier
 * File Name: VideoGameModel.cs
 * Project Name: InventoryManagerUI
 * Creation Date: 03/11/2020
 * Modified Date: 03/23/2020
 * Description: A video game model data class that inherits all the properties of the base inventory model.
 */

using DevExpress.Mvvm.DataAnnotations;
using System.ComponentModel;

namespace InventoryManagerUI.Models
{
    /// <summary>
    /// A video game model data class that inherits all the properties of the base inventory model.
    /// </summary>
    [POCOViewModel]
    public class VideoGameModel : BaseInventoryModel
    {
        #region Properties

        /// <summary>
        /// The developer of the video game.
        /// </summary>
        public virtual string Developer { get; set; }

        /// <summary>
        /// The rating of the video game.
        /// </summary>
        public virtual float? Rating { get; set; }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// An override for GetError from the base inventory model that returns an error relevant to the video game model.
        /// </summary>
        /// <returns>A message in the case of an error, or null otherwise</returns>
        protected override string GetError()
        {
            // Check all the video game model specific properties for errors.
            if (this[nameof(Developer)] != null || this[nameof(Rating)] != null)
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
                    case nameof(Developer):
                        if (string.IsNullOrWhiteSpace(Developer))
                        {
                            return "Developer is required.";
                        }

                        break;
                        
                    // Return an appropriate error message if a value-type property is null.
                    case nameof(Rating):
                        if (Rating == null)
                        {
                            return "Rating is required.";
                        }

                        break;
                }

                // Check the property in the base class if it is not a video game model specific property.
                return base[columnName];
            }
        }

        /// <summary>
        /// Copies all the properties from a source inventory model into this video game model.
        /// </summary>
        /// <param name="source"></param>
        public override void CopyFrom(BaseInventoryModel source)
        {
            // Copy the base properties from the source.
            base.CopyFrom(source);

            // Check if the source inventory model is a video game model and copy the specific video game properties from the videoGameSource.
            if (source is VideoGameModel videoGameSource)
            {
                Developer = videoGameSource.Developer;
                Rating = videoGameSource.Rating;
            }
        }

        #endregion // Methods
    }
}