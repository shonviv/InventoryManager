/*
 * Author: Shon Vivier
 * File Name: InventoryMapper.cs
 * Project Name: InventoryManagerUI
 * Creation Date: 03/11/2020
 * Modified Date: 03/23/2020
 * Description: A helper class that maps inventory item objects and inventory item models.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Mvvm.POCO;
using Entities;
using InventoryManagerUI.Models;

namespace InventoryManagerUI.Mappers
{
    /// <summary>
    /// A helper class that maps inventory item objects and inventory item models.
    /// </summary>
    public class InventoryMapper
    {
        #region Public Methods

        /// <summary>
        /// Maps from a list of one type TK to a list of a another type T using a mapper function designated while calling the method.
        /// </summary>
        /// <typeparam name="TIn">A type that we map from.</typeparam>
        /// <typeparam name="TOut">A type that we map to.</typeparam>
        /// <param name="items">The inventory item objects or models to be mapped.</param>
        /// <param name="mapper">The function used for mapping.</param>
        /// <returns></returns>
        public IReadOnlyList<TIn> Map<TIn, TOut>(IReadOnlyList<TOut> items, Func<TOut, TIn> mapper)
        {
            // If the input collection is empty, return a new empty list of the target type.
            // Otherwise, preform the individual mapping.
            return items?.Any() == true ? items.Select(mapper).ToList() : new List<TIn>();
        }

        /// <summary>
        /// Maps a book object to a book model.
        /// </summary>
        /// <param name="book">The book being mapped to a book model.</param>
        /// <returns>A new book model with all of the relevant properties of the book.</returns>
        public static BookModel MapToBookModel(Book book)
        {
            // Create a new book model.
            BookModel bookModel = CreateNewModel<BookModel>();

            // Map all the common properties to the book model.
            BaseInventoryModel baseModel = bookModel;
            MapCommonModel(book, ref baseModel);

            // Map the unique book properties to the book model.
            bookModel.Author = book.Author;
            bookModel.Publisher = book.Publisher;

            return bookModel;
        }

        /// <summary>
        /// Maps a movie to a movie model.
        /// </summary>
        /// <param name="movie">The movie being mapped to a movie model.</param>
        /// <returns>A new movie model with all of the relevant properties of the movie.</returns>
        public static MovieModel MapToMovieModel(Movie movie)
        {
            // Create a new movie model.
            MovieModel movieModel = CreateNewModel<MovieModel>();

            // Map all the common properties to the movie model.
            BaseInventoryModel baseModel = movieModel;
            MapCommonModel(movie, ref baseModel);

            // Map the unique movie properties to the movie model.
            movieModel.Director = movie.Director;
            movieModel.Duration = movie.Duration;

            return movieModel;
        }

        /// <summary>
        /// Maps a video game to a video game model.
        /// </summary>
        /// <param name="game">The video game being mapped to a video game model.</param>
        /// <returns>A new video game model with all of the relevant properties of the video game.</returns>
        public static VideoGameModel MapToVideoGameModel(VideoGame game)
        {
            // Create a new video game model.
            VideoGameModel gameModel = CreateNewModel<VideoGameModel>();

            // Map all the common properties to the video game model.
            BaseInventoryModel baseModel = gameModel;
            MapCommonModel(game, ref baseModel);

            // Map the unique video game properties to the video game model.
            gameModel.Developer = game.Developer;
            gameModel.Rating = game.Rating;

            return gameModel;
        }

        /// <summary>
        /// Maps a book model back to a book.
        /// </summary>
        /// <param name="bookModel">The book model being mapped back to a book.</param>
        /// <returns>A new book model with all of the relevant properties of the book model.</returns>
        public static Book MapToBook(BookModel bookModel)
        {
            return new Book(bookModel.Title,
                bookModel.Cost.GetValueOrDefault(),
                bookModel.Genre,
                bookModel.Platform,
                bookModel.ReleaseYear.GetValueOrDefault(),
                bookModel.Author,
                bookModel.Publisher);
        }

        /// <summary>
        /// Maps a movie model back to a movie.
        /// </summary>
        /// <param name="movieModel">The movie model being mapped back to a movie.</param>
        /// <returns>A new movie model with all of the relevant properties of the movie model.</returns>
        public static Movie MapToMovie(MovieModel movieModel)
        {
            return new Movie(movieModel.Title,
                movieModel.Cost.GetValueOrDefault(),
                movieModel.Genre,
                movieModel.Platform,
                movieModel.ReleaseYear.GetValueOrDefault(),
                movieModel.Director,
                movieModel.Duration.GetValueOrDefault());
        }
        
        /// <summary>
        /// Maps a video game model back to a video game.
        /// </summary>
        /// <param name="videoGameModel">The video game model being mapped back to a video game.</param>
        /// <returns>A new video game model with all of the relevant properties of the video game model.</returns>
        public static VideoGame MapToVideoGame(VideoGameModel videoGameModel)
        {
            return new VideoGame(videoGameModel.Title,
                videoGameModel.Cost.GetValueOrDefault(),
                videoGameModel.Genre,
                videoGameModel.Platform,
                videoGameModel.ReleaseYear.GetValueOrDefault(),
                videoGameModel.Developer,
                videoGameModel.Rating.GetValueOrDefault());
        }

        #endregion // Public Methods

        #region Private Methods

        /// <summary>
        /// Maps all the common properties in an inventory item's base class to an inventory model's base class.
        /// </summary>
        /// <param name="sourceItem">The inventory item we are mapping to a model.</param>
        /// <param name="targetModel">A reference to the model we are mapping the base inventory item's properties to.</param>
        private static void MapCommonModel(BaseInventoryItem sourceItem, ref BaseInventoryModel targetModel)
        {
            // Throw exceptions if any parameters are null.
            if (sourceItem == null)
            {
                throw new ArgumentNullException(nameof(sourceItem));
            }

            if (targetModel == null)
            {
                throw new ArgumentNullException(nameof(targetModel));
            }

            // Set all the common properties of the inventory model from the source inventory item.
            targetModel.Title = sourceItem.Title;
            targetModel.Cost = sourceItem.Cost;
            targetModel.Genre = sourceItem.Genre;
            targetModel.Platform = sourceItem.Platform;
            targetModel.ReleaseYear = sourceItem.ReleaseYear;
        }

        /// <summary>
        /// Creates a new view model of type T.
        /// </summary>
        /// <typeparam name="T">A type representing any view model type (i.e book, movie, video game, etc.)</typeparam>
        /// <returns>A new model of type T</returns>
        private static T CreateNewModel<T>()
        {
            // DevExpress uses Reflection Emit﻿ to create a descendant of the specified ViewModel class and returns the descendant class instance at runtime.
            // We pass a new emitted type to the activator to create a new corresponding instance.
            return (T)Activator.CreateInstance(ViewModelSource.GetPOCOType(typeof(T)));
        }

        #endregion // Private Methods
    }
}