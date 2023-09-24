using CollectionViewLayoutWrongApp.DatabaseModels;

namespace CollectionViewLayoutWrongApp.ViewModels
{
    public partial class GroupedSavedItemList : List<SavedItem>
    {
        public string GroupName { get; set; }

        public GroupedSavedItemList(string groupName, List<SavedItem> item) : base(item)
        {
            GroupName = groupName;
        }
    }
}
