using CollectionViewLayoutWrongApp.DatabaseModels;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.ComponentModel;

namespace CollectionViewLayoutWrongApp.ViewModels
{
    public partial class CustomPopupViewModel : ViewModelBase, INotifyPropertyChanged 
    {
        public ObservableRangeCollection<SavedItem> SavedItems { get; set; }
                
        
        public AsyncCommand<bool> RefreshCommand => new AsyncCommand<bool>(RefreshDatabase);
        public AsyncCommand<SavedItem> DeleteSavedItemCommand => new AsyncCommand<SavedItem>(DeleteSavedItem);
        public AsyncCommand OkButtonCommand { get; }

                

        public string SelectedSavedItemName { get; set; }

        public SavedItem SelectedSavedItem { get; set; }


        public CustomPopupViewModel()
        {
            SavedItems = new ObservableRangeCollection<SavedItem>();

            OkButtonCommand = new AsyncCommand(OnOkButtonCommand);

            RefreshCommand.ExecuteAsync(true);
        }

        private IEnumerable<SavedItem> GetSavedItems()
        {
            var savedItems = new List<SavedItem>();
            savedItems.Add(new SavedItem { Name = "A", LastSavedDate = DateTime.Now });

            return savedItems;
        }

        private async Task RefreshDatabase(bool selectSavedItem = true)
        {
            IsBusy = true;

            //await Task.Delay(500);

            var savedItems = GetSavedItems();
            SavedItems.Clear();
            SavedItems.AddRange(savedItems);

            GroupSavedItemsWithSearchFilter();       // Start with empty Search-Filter (=> get all Items)

            if (selectSavedItem)
            {
                // In case we have a name for an item to select, we try to select it,
                // otherwise select element with Index = 0      
                await SelectSavedItemByName(SelectedSavedItemName);
            }
            
            IsBusy = false;

            await Task.CompletedTask;
        }

        private async Task SelectSavedItemByName(string savedItemName)
        {
            if (string.IsNullOrWhiteSpace(savedItemName))
            {
                await SelectSavedItemByIndex(0);
                return;
            }

            SavedItem foundSavedItem = GroupedSavedItems.SelectMany(list => list).FirstOrDefault(savedItem => savedItem.Name == savedItemName);

            SelectedSavedItem = foundSavedItem;
            OnPropertyChanged(nameof(SelectedSavedItem));
            
            await Task.CompletedTask;
        }

        private async Task SelectSavedItemByIndex(int index)
        {
            if (SavedItems.Count == 0)
            {
                SelectedSavedItem = null;
            }
            else if (SavedItems.Count >= 1)
            {
                // In case there is at least one Element left and we want
                // to select an index out of the ranges we set it to the
                // corresponding lower or upper range value
                if (index < 0)
                {
                    index = 0;
                }
                else if (index >= SavedItems.Count)
                {
                    index = SavedItems.Count - 1;
                }

                var selectSuccessor = SavedItems[index];
                await SelectSavedItemByName(selectSuccessor.Name);
            }

            await Task.CompletedTask;
        }

        private async Task DeleteSavedItem(SavedItem itemToDelete)
        {
            await Task.CompletedTask;
        }

        #region Search Filter

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    OnSearchTextChanged();
                }
            }
        }

        protected virtual void OnSearchTextChanged()
        {
            GroupSavedItemsWithSearchFilter();
        }

        #endregion

        #region Grouping of SavedItems

        private ObservableRangeCollection<GroupedSavedItemList> _groupedSavedItems { get; set; }
        public ObservableRangeCollection<GroupedSavedItemList> GroupedSavedItems
        {
            get => _groupedSavedItems != null ? _groupedSavedItems : new ObservableRangeCollection<GroupedSavedItemList>();
            set
            {
                if (_groupedSavedItems != value)
                {
                    _groupedSavedItems = value;
                    OnPropertyChanged(nameof(GroupedSavedItems));
                }
            }
        }

        private void GroupSavedItemsWithSearchFilter()
        {
            IOrderedEnumerable<KeyValuePair<string, List<SavedItem>>> groupedSavedItemsDictionary;
            if (string.IsNullOrEmpty(SearchText))
            {
                groupedSavedItemsDictionary = SavedItems
                    .OrderBy(savedItem => savedItem.Name)
                    .GroupBy(savedItem => savedItem.Name.ToUpperInvariant().Substring(0, 1))
                    .ToDictionary(group => group.Key, group => group.ToList()).OrderBy(group => group.Key);
            }
            else
            {
                groupedSavedItemsDictionary = SavedItems
                    .Where(savedItem => savedItem.Name.ToLower().StartsWith(SearchText.ToLower()))
                    .OrderBy(savedItem => savedItem.Name)
                    .GroupBy(savedItem => savedItem.Name.ToUpperInvariant().Substring(0, 1))
                    .ToDictionary(group => group.Key, group => group.ToList()).OrderBy(group => group.Key);
            }

            _groupedSavedItems = new ObservableRangeCollection<GroupedSavedItemList>();

            foreach (KeyValuePair<string, List<SavedItem>> item in groupedSavedItemsDictionary)
            {
                _groupedSavedItems.Add(new GroupedSavedItemList(item.Key, new List<SavedItem>(item.Value)));
            }

            OnPropertyChanged(nameof(GroupedSavedItems));
        }

        #endregion

        #region OK Button Command

        private async Task OnOkButtonCommand()
        {
            await Task.CompletedTask;
        }

        #endregion        
    }
}
