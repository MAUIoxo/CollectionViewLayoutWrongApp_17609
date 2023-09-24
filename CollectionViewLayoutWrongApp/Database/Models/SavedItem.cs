namespace CollectionViewLayoutWrongApp.DatabaseModels
{
    public class SavedItem
    {
        public int Id { get; set; }


        public string Name { get; set; } 

        public DateTime LastSavedDate { get; set; } = DateTime.Now;
    }
}
