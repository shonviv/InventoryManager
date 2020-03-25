/*
 * Author: Shon Vivier
 * File Name: FindItemViewModel.cs
 * Project Name: InventoryManagerUI
 * Creation Date: 03/11/2020
 * Modified Date: 03/23/2020
 * Description: The main view model data context for the main view. 
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DataAccess.Interfaces;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using Entities;
using Entities.Interfaces;
using InventoryManagerUI.Mappers;
using InventoryManagerUI.Models;
using Microsoft.Extensions.Logging;

namespace InventoryManagerUI.ViewModels
{
    /// <summary>
    /// The main view model data context for the main view.
    /// </summary>
    [POCOViewModel]
    public class MainViewModel
    {
        #region Fields

        // Represents the name or path of the file we are searching for from the bin file.
        private const string FileName = "inventory.txt";

        // Used to write logs.
        private readonly ILogger _logger;

        // Used to access our file data source and load/save all the inventory item objects.
        private readonly IInventoryRepository _repository;

        // Maps between inventory items and models.
        private readonly InventoryMapper _mapper;
        
        #endregion // Fields

        #region Constructors

        /// <summary>
        /// Initializes the logger, inventory repository, inventory mapper, and loads in all the inventory data.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        public MainViewModel(ILogger<MainViewModel> logger, IInventoryRepository repository, InventoryMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            
            // Load in the repository
            _ = LoadAsync();
        }

        #endregion // Constructors

        #region Properties

        // Allows us to create message box pop-ups.
        protected virtual IMessageBoxService MessageBoxService => null;

        // Creates a new dialog window for adding or modifying inventory items.
        [ServiceProperty(Key = "AddOrModifyItemDialogService")]
        protected virtual IDialogService AddOrModifyItemDialogService => null;

        // Creates a new dialog window for finding items.
        [ServiceProperty(Key = "FindItemDialogService")]
        protected virtual IDialogService FindItemDialogService => null;

        // Observable collection of all the different inventory view models.
        public virtual ObservableCollection<BookModel> BookModels { get; protected set; }

        public virtual ObservableCollection<MovieModel> MovieModels { get; protected set; }

        public virtual ObservableCollection<VideoGameModel> VideoGameModels { get; protected set; }
        
        // The inventory model that is being highlighted in the view.
        public virtual BaseInventoryModel FocusedItemModel { get; set; }

        // True whenever the program is currently loading/is busy with a task (i.e saving or loading).
        // Binded to the view, showing a loading screen when appropriate.
        public virtual bool IsLoading { get; protected set; }

        #endregion // Properties

        #region Private Methods

        /// <summary>
        /// Loads in the data from the inventory repository. The function is asynchronous so that the view is still
        /// displayed and partially functional while items are loading in.
        /// </summary>
        /// <returns>A new thread.</returns>
        private async Task LoadAsync()
        {
            // Indicate that we are currently loading.
            IsLoading = true;

            // Try to load in the data.
            try
            {
                // Create a new thread.
                await Task.Run(() =>
                {
                    // Load in all the inventory items from the repository.
                    IReadOnlyList<IInventoryItem> inventory = _repository.LoadInventory(FileName);

                    // Map the inventory items to their appropriate models.
                    IReadOnlyList<BookModel> books = _mapper.Map(inventory.OfType<Book>().ToList(),
                        InventoryMapper.MapToBookModel);
                    IReadOnlyList<MovieModel> movies = _mapper.Map(inventory.OfType<Movie>().ToList(),
                        InventoryMapper.MapToMovieModel);
                    IReadOnlyList<VideoGameModel> videoGames = _mapper.Map(inventory.OfType<VideoGame>().ToList(),
                        InventoryMapper.MapToVideoGameModel);

                    // Initialize the model collection properties in the view model.
                    BookModels = new ObservableCollection<BookModel>(books);
                    MovieModels = new ObservableCollection<MovieModel>(movies);
                    VideoGameModels = new ObservableCollection<VideoGameModel>(videoGames);
                }).ConfigureAwait(true);
            }
            catch (Exception e)
            {
                // In the case of an exception, display that there was an issue with loading.
                IsLoading = false;

                MessageBoxService.Show("Failed to load the inventory.", "Inventory Manager", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                _logger.LogError(1, e, "Failed to load the inventory.");
            }

            IsLoading = false;
        }

        #endregion // Private Methods

        #region Commands

        // NOTE: The DevExpress POCO mechanism generates commands for all public methods without parameters or with a single parameter by convention.
        // Corresponding delegate command properties are created using the names of public methods with the suffix "Command".

        /// <summary>
        /// Creates a new dialog window that allows the user to add an item.
        /// </summary>
        /// <param name="itemType">The type of item is bound to the the tab the user is on (i.e adding from the movie tab creates a new movie).</param>
        public void AddItem(InventoryItemType itemType)
        {
            // Get the type of view model we are creating.
            Type viewModelType;

            switch (itemType)
            {
                case InventoryItemType.Book:
                    viewModelType = typeof(BookModel);
                    break;

                case InventoryItemType.Movie:
                    viewModelType = typeof(MovieModel);
                    break;

                case InventoryItemType.VideoGame:
                    viewModelType = typeof(VideoGameModel);
                    break;

                // Throw an exception if our item type does not match any of the models.
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Resolve (create) the view model.
            if (!(App.ServiceProvider.GetService(viewModelType) is BaseInventoryModel viewModel))
            {
                return;
            }

            // Create a new button that adds the item.
            UICommand addItemCommand = new UICommand()
            {
                Caption = "Add",
                IsDefault = true,

                // Only allow the user to add the item if there are no errors with the data entry.
                Command = new DelegateCommand(
                    () => { },
                    () => string.IsNullOrEmpty(viewModel.Error)
                )
            };

            // Create a new button that cancels and closes the dialog window for adding an item.
            UICommand cancelCommand = new UICommand()
            {
                Caption = "Cancel",
                IsCancel = true,
            };

            // Display the dialog window.
            UICommand result = AddOrModifyItemDialogService?.ShowDialog(
                dialogCommands: new[] {addItemCommand, cancelCommand},
                title: "Add a New Item",
                viewModel: viewModel
            );

            // Check if the user executed the add command (clicked the add button).
            if (result != addItemCommand)
            {
                return;
            }

            // Add the new model based on the type of item we are adding.
            switch (itemType)
            {
                case InventoryItemType.Book:
                    BookModels.Add(viewModel as BookModel);
                    break;

                case InventoryItemType.Movie:
                    MovieModels.Add(viewModel as MovieModel);
                    break;

                case InventoryItemType.VideoGame:
                    VideoGameModels.Add(viewModel as VideoGameModel);
                    break;

                // Throw an exception if our item type does not match any of the models.
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Highlight the newly added item.
            FocusedItemModel = viewModel;
        }

        /// <summary>
        /// Determines whether or not a new item can be currently added. Disables the Add Item button if false.
        /// </summary>
        /// <param name="itemType">The type of item is bound to the the tab the user is on (i.e adding from the movie tab creates a new movie).</param>
        /// <returns>A boolean value indicating whether or not a new item can be added.</returns>
        public bool CanAddItem(InventoryItemType itemType)
        {
            // The user can add a new item if the main window is not loading (i.e saving or loading a repository).
            return !IsLoading;
        }

        /// <summary>
        /// Creates a new dialog window that allows the user to find an item.
        /// </summary>
        /// <param name="itemType">The type of item is bound to the the tab the user is on (i.e searching from the movie tab returns a movie).</param>
        public void FindItem(InventoryItemType itemType)
        {
            // Try to resolve FindItemViewModel.
            if (!(App.ServiceProvider.GetService(typeof(FindItemViewModel)) is FindItemViewModel viewModel))
            {
                return;
            }
            
            // Create a new button that finds the item.
            UICommand findItemCommand = new UICommand()
            {
                Caption = "Search",
                IsDefault = true,

                // Only allow the user to find an item if there are no errors in the data entry.
                Command = new DelegateCommand(
                    () => { },
                    () => !string.IsNullOrWhiteSpace(viewModel.Title) && !string.IsNullOrWhiteSpace(viewModel.Platform)
                )
            };

            // Create a new button that cancels and closes the dialog window for finding an item.
            UICommand cancelCommand = new UICommand()
            {
                Caption = "Cancel",
                IsCancel = true,
            };

            // Display the dialog window.
            UICommand result = FindItemDialogService?.ShowDialog(
                dialogCommands: new[] {findItemCommand, cancelCommand},
                title: "Find an Item",
                viewModel: viewModel
            );

            // Check if the user executed the find command (clicked the find button).
            if (result != findItemCommand)
            {
                return;
            }

            // Switch based on the tab the user is on.
            switch (itemType)
            {
                case InventoryItemType.Book:
                    // Return the first item that has the requested title and platform of the inventory item.
                    FocusedItemModel = BookModels.FirstOrDefault(x =>
                        string.Equals(x.Title, viewModel.Title, StringComparison.InvariantCultureIgnoreCase) &&
                        string.Equals(x.Platform, viewModel.Platform, StringComparison.InvariantCultureIgnoreCase));
                    break;

                case InventoryItemType.Movie:
                    FocusedItemModel = MovieModels.FirstOrDefault(x =>
                        string.Equals(x.Title, viewModel.Title, StringComparison.InvariantCultureIgnoreCase) &&
                        string.Equals(x.Platform, viewModel.Platform, StringComparison.InvariantCultureIgnoreCase));
                    break;

                case InventoryItemType.VideoGame:
                    FocusedItemModel = VideoGameModels.FirstOrDefault(x =>
                        string.Equals(x.Title, viewModel.Title, StringComparison.InvariantCultureIgnoreCase) &&
                        string.Equals(x.Platform, viewModel.Platform, StringComparison.InvariantCultureIgnoreCase));
                    break;

                // Throw an exception if our item type does not match any of the models.
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // If a model was not found, display a new message box indicating so to the user.
            if (FocusedItemModel == null)
            {
                MessageBoxService?.ShowMessage($"Could not find \"{viewModel.Title}\" on \"{viewModel.Platform}\"");
            }
        }

        /// <summary>
        /// Determines whether or not you can search for an item. Disables the Find Item button if false.
        /// </summary>
        /// <param name="itemType">The type of item is bound to the the tab the user is on (i.e adding from the movie tab creates a new movie).</param>
        /// <returns>A boolean value indicating whether or not the user can search for an item.</returns>
        public bool CanFindItem(InventoryItemType itemType)
        {
            return !IsLoading;
        }

        /// <summary>
        /// Creates a new dialog window that allows a user to edit a specific inventory item.
        /// </summary>
        /// <param name="item">The inventory item being edited.</param>
        public void ModifyItem(BaseInventoryModel item)
        {
            // Get the type of view model we are creating.
            Type viewModelType;

            // Get the type of model we need.
            if (item is MovieModel)
            {
                viewModelType = typeof(MovieModel);
            }
            else if (item is BookModel)
            {
                viewModelType = typeof(BookModel);
            }
            else if (item is VideoGameModel)
            {
                viewModelType = typeof(VideoGameModel);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(viewModelType), @"Unknown type for inventory model");
            }
            
            // Resolve (create) the view model.
            if (!(App.ServiceProvider.GetService(viewModelType) is BaseInventoryModel viewModel))
            {
                return;
            }

            // Copy the item's properties into the new model.
            viewModel.CopyFrom(item);

            // Create a new button to save changes to the item.
            UICommand saveChangesCommand = new UICommand()
            {
                Caption = "Save",
                IsDefault = true,

                // Only allow the user to save their changes if there are no errors in the data entry.
                Command = new DelegateCommand(
                    () => { },
                    () => string.IsNullOrEmpty(viewModel.Error)
                )
            };

            // Create a new button that cancels and closes the dialog window for modifying an item.
            UICommand cancelCommand = new UICommand()
            {
                Caption = "Cancel",
                IsCancel = true,
            };

            // Display the dialog window.
            UICommand result = AddOrModifyItemDialogService?.ShowDialog(
                dialogCommands: new[] { saveChangesCommand, cancelCommand },
                title: "Modify an Item",
                viewModel: viewModel
            );

            // Check if the user executed the save command (clicked the save button).
            if (result != saveChangesCommand)
            {
                return;
            }

            // Copy the properties from the item we modified back into the original inventory item.
            item.CopyFrom(viewModel);
        }

        /// <summary>
        /// Determines whether or not an item can be currently modified. Disables the Modify Item button if false.
        /// </summary>
        /// <param name="item">The item we are modifying.</param>
        /// <returns>A boolean value indicating whether or not an item can be modified.</returns>
        public bool CanModifyItem(BaseInventoryModel item)
        {
            // The user can modify an item if the item exists and the main window is not loading (i.e saving or loading a repository).
            return item != null && !IsLoading;
        }

        /// <summary>
        /// Deletes a specific item.
        /// </summary>
        /// <param name="item">The item being deleted.</param>
        public void DeleteItem(BaseInventoryModel item)
        {
            // Remove the item from their respective model collection.
            if (item is MovieModel movieModel)
            {
                MovieModels.Remove(movieModel);
            }
            else if (item is BookModel bookModel)
            {
                BookModels.Remove(bookModel);
            }
            else if (item is VideoGameModel videoGameModel)
            {
                VideoGameModels.Remove(videoGameModel);
            }
        }

        /// <summary>
        /// Determines whether or not an item can be currently deleted. Disables the Delete Item button if false.
        /// </summary>
        /// <param name="item">The item being deleted.</param>
        /// <returns>A boolean value indicating whether or not the item can be deleted.</returns>
        public bool CanDeleteItem(BaseInventoryModel item)
        {
            // The user can delete an item if the item exists and the main window is not loading (i.e saving or loading a repository).
            return item != null && !IsLoading;
        }

        /// <summary>
        /// Saves the data from each model collection into the inventory repository. The function is asynchronous so that 
        /// the view is still displayed and partially functional while items are saving. 
        /// </summary>
        /// <returns>A new thread for an async command.</returns>
        public async Task Save()
        {
            // Indicate that we are currently loading.
            IsLoading = true;

            // Try to save the data.
            try
            {
                // Create a new thread.
                await Task.Run(() =>
                    {
                        // Convert all of the inventory models back to item objects.
                        IReadOnlyList<Book> books = _mapper.Map(BookModels, InventoryMapper.MapToBook);
                        IReadOnlyList<Movie> movies = _mapper.Map(MovieModels, InventoryMapper.MapToMovie);
                        IReadOnlyList<VideoGame> videoGames =
                            _mapper.Map(VideoGameModels, InventoryMapper.MapToVideoGame);

                        // Store each of them in a new list of inventory items.
                        List<BaseInventoryItem> list = new List<BaseInventoryItem>();
                        list.AddRange(books);
                        list.AddRange(movies);
                        list.AddRange(videoGames);

                        // Save the inventory with the list.
                        _repository.SaveInventory(FileName, list);
                    })
                    .ConfigureAwait(true);

                // Finish loading and indicate that the save was successful to the user.
                IsLoading = false;

                MessageBoxService?.Show("Save Successful", "Inventory Manager", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                // In the case of an exception, display that there was an issue with loading.
                IsLoading = false;

                MessageBoxService?.Show("Failed to save the inventory", "Inventory Manager", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                _logger.LogError(1, e, "Failed to save the inventory.");
            }
        }

        /// <summary>
        /// Determines whether or not we can save the inventory. Disables the Save button if false.
        /// </summary>
        /// <returns>A boolean value indicating whether or not the inventory can be saved.</returns>
        public bool CanSave()
        {
            // The user can save if the main window is not loading (i.e saving or loading a repository).
            return !IsLoading;
        }

        #endregion // Commands
    }
}