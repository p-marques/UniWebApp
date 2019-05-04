namespace UniWebApp.Core
{
    public class AppEntityDataFieldComboboxOption
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation
        public AppEntityDataFieldCombobox Combobox { get; set; }
    }
}
