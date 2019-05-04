using System.ComponentModel.DataAnnotations;

namespace UniWebApp.Core
{
    public class AppEntityDataFieldComboboxOption
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public AppEntityDataFieldCombobox Combobox { get; set; }
    }
}