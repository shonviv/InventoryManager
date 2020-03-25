/*
 * Author: Shon Vivier
 * File Name: FileInventoryRepository.cs
 * Project Name: DataAccess
 * Creation Date: 03/09/2020
 * Modified Date: 03/22/2020
 * Description: Handles the logic required to access file-based data sources (loading and saving inventory from/to a text file). 
 */

using System;
using System.Collections.Generic;
using System.IO;
using DataAccess.Interfaces;
using Entities;
using Entities.Interfaces;
using Entities.Utilities;

namespace DataAccess
{
    /// <summary>
    /// Handles loading and saving a file-based inventory.
    /// </summary>
    public class FileInventoryRepository : IInventoryRepository
    {
        #region Methods

        /// <summary>
        /// Reads in a text file and creates a list of inventory objects based on the input.
        /// The input format is as follows: 'itemType,title,cost,genre,platform,releaseYear,typeSpecificInfo1,typeSpecificInfo2'
        /// </summary>
        /// <param name="path">The file's location.</param>
        /// <returns>A read-only list of inventory item objects.</returns>
        public IReadOnlyList<IInventoryItem> LoadInventory(string path)
        {
            // Make sure that the file exists and throw an exception if it doesn't
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File in path \"{path}\" was not found");
            }

            // Read in the entire text inventory file.
            string[] inventoryText = File.ReadAllLines(path);

            // Create a new empty list of inventory items to return.
            List<IInventoryItem> inventory = new List<IInventoryItem>();

            // Iterate over each line in the file.
            foreach (string line in inventoryText)
            {
                // A try-catch block to check if input line can be transformed into a valid inventory item object.
                try
                {
                    // Get the inventory item from text and add it to the inventory if it is not null.
                    BaseInventoryItem item = InventoryItemFactory.CreateInventoryItem(line);
                    if (item != null)
                    {
                        inventory.Add(item);
                    }
                }
                catch
                {
                    // Skip any invalid entries.
                }
            }

            // Return the loaded inventory as a read-only list of inventory item objects.
            return inventory;
        }

        /// <summary>
        /// Reads in a list of inventory item objects and saves them to a specified file in a text format.
        /// </summary>
        /// <param name="path">The file's location.</param>
        /// <param name="items">The list of item objects to be saved.</param>
        /// <returns>A boolean value representing whether or not the save was successful.</returns>
        public bool SaveInventory(string path, IReadOnlyList<BaseInventoryItem> items)
        {
            // Check to see if either the path or items do not exist and throw appropriate exceptions.
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            
            // Creates a unique, zero-byte temporary file on disk and stores the full path of the file.
            string tempFile = Path.GetTempFileName();
            
            // Try to write all the inventory item objects to the temporary file and replace the file at the specified path with the temporary file.
            try
            {
                // Write each line to the temporary file.
                using (StreamWriter sw = new StreamWriter(tempFile))
                {
                    foreach (BaseInventoryItem item in items)
                    {
                        // Each inventory item has an overriden ToString() method that automatically displays the properties of the object in a string format.
                        sw.WriteLine(item.ToString());
                    }
                }

                // Ensure that the target file still exists.
                if (File.Exists(path))
                {
                    // Delete the target file.
                    File.Delete(path);
                }

                // Move the newly created file to the old file's location.
                File.Move(tempFile, path);

                // Return a boolean indicating whether or not the save was successful.
                // If the code reached the end of the try statement without an exception, it will return true.
                return true;
            }
            finally
            {
                // Delete the temporary from the disk after replacing the old inventory file.
                File.Delete(tempFile);
            }
        }

        #endregion // Methods
    }
}