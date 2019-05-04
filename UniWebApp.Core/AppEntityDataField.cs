namespace UniWebApp.Core
{
    public class AppEntityDataField
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation
        public AppEntity Entity { get; set; }
    }
}