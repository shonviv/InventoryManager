/*
 * Author: Shon Vivier
 * File Name: VideoGame.cs
 * Project Name: Entities
 * Creation Date: 03/09/2020
 * Modified Date: 03/22/2020
 * Description: The video game data class that inherits all the functionality of a base inventory item.
 */

namespace Entities
{
    /// <summary>
    /// The video game data class. Implements all the functionality of a base inventory item.
    /// </summary>
    public class VideoGame : BaseInventoryItem
    {
        #region Constructor

        /// <summary>
        /// Initializes all the properties of the video game and the base inventory item.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="cost"></param>
        /// <param name="genre"></param>
        /// <param name="platform"></param>
        /// <param name="releaseYear"></param>
        /// <param name="developer"></param>
        /// <param name="rating"></param>
        public VideoGame(string title, decimal cost, string genre, string platform, uint releaseYear, string developer, float rating)
            : base(title, cost, genre, platform, releaseYear)
        {
            Developer = developer;
            Rating = rating;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// The developer of the video game.
        /// </summary>
        public string Developer { get; }

        /// <summary>
        /// The rating of the video game. A float is used to allow for tweener values (i.e 9.5/10)
        /// </summary>
        public float Rating { get; }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// An override of ToString() that returns all of the video game's properties into a file-ready string format.
        /// </summary>
        /// <returns>A string containing all the properties of the video game in a file-ready format.</returns>
        public override string ToString()
        {
            return $"Game,{Title},{Cost},{Genre},{Platform},{ReleaseYear},{Developer},{Rating}";
        }

        #endregion // Methods
    }
}