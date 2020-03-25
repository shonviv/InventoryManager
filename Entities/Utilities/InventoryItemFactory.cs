/*
 * Author: Shon Vivier
 * File Name: InventoryItemFactory.cs
 * Project Name: Entities
 * Creation Date: 03/10/2020
 * Modified Date: 03/22/2020
 * Description: A factory class to create inventory DTO items.
 */

using System;

namespace Entities.Utilities
{
    /// <summary>
    /// A factory class to create inventory DTO items.
    /// </summary>
    public static class InventoryItemFactory
    {
        /// <summary>
        /// Creates an inventory item object from a string of text.
        /// </summary>
        /// <param name="line">The input format is as follows: 'itemType,title,cost,genre,platform,releaseYear,typeSpecificInfo1,typeSpecificInfo2'</param>
        /// <returns>Returns a BaseInventoryItem if successful, and null if failed.</returns>
        public static BaseInventoryItem CreateInventoryItem(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return null;
            }
            
            // Split the string by a comma and store all of the base inventory properties.
            string[] split = line.Split(',');

            string itemType = split[0];
            string title = split[1];
            string genre = split[3];
            string platform = split[4];

            // Value types use Parse rather than TryParse because we expect that all input data should be in a valid format.
            // Non-valid data enters the catch block and is appropriately logged and skipped.
            decimal cost = decimal.Parse(split[2]);
            uint releaseYear = uint.Parse(split[5]);

            // Create a new inventory object based on the specified type.
            switch (itemType)
            {
                case "Book":
                    // Parse type-specific data.
                    string author = split[6];
                    string publisher = split[7];

                    // Create a new type-specific inventory object and add it to the inventory.
                    return new Book(title, cost, genre, platform, releaseYear, author, publisher);

                case "Movie":
                    string director = split[6];
                    TimeSpan duration = TimeSpan.FromMinutes(int.Parse(split[7]));

                    return new Movie(title, cost, genre, platform, releaseYear, director, duration);

                case "Game":
                    string developer = split[6];
                    float rating = float.Parse(split[7]);

                    return new VideoGame(title, cost, genre, platform, releaseYear, developer, rating);

                // Return null to indicate that converting failed.
                default:
                    return null;
            }
        } 
    }
}