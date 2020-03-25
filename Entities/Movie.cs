/*
 * Author: Shon Vivier
 * File Name: Movie.cs
 * Project Name: Entities
 * Creation Date: 03/09/2020
 * Modified Date: 03/22/2020
 * Description: The movie data class that inherits all the functionality of a base inventory item.
 */

using System;

namespace Entities
{
    /// <summary>
    /// The movie data class. Implements all the functionality of a base inventory item.
    /// </summary>
    public class Movie : BaseInventoryItem
    {
        #region Constructor

        /// <summary>
        /// Initializes all the properties of the movie and the base inventory item.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="cost"></param>
        /// <param name="genre"></param>
        /// <param name="platform"></param>
        /// <param name="releaseYear"></param>
        /// <param name="director"></param>
        /// <param name="duration"></param>
        public Movie(string title, decimal cost, string genre, string platform, uint releaseYear, string director, TimeSpan duration)
            : base(title, cost, genre, platform, releaseYear)
        {
            Director = director;
            Duration = duration;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// The director of the movie.
        /// </summary>
        public string Director { get; }

        /// <summary>
        /// The duration of the movie. The duration value is initially parsed in minutes from the file and is converted into a TimeSpan
        /// for simpler work converting between different times (hours, minutes, seconds, etc.)
        /// </summary>
        public TimeSpan Duration { get; }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// An override of ToString() that returns all of the movie's properties into a file-ready string format.
        /// </summary>
        /// <returns>A string containing all the properties of the movie in a file-ready format.</returns>
        public override string ToString()
        {
            return $"Movie,{Title},{Cost},{Genre},{Platform},{ReleaseYear},{Director},{Duration.TotalMinutes}";
        }

        #endregion // Methods
    }
}