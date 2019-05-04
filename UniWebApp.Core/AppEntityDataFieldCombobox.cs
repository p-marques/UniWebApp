using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniWebApp.Core
{
    public class AppEntityDataFieldCombobox : AppEntityDataField
    {
        [Required]
        public AppEntityDataFieldComboboxOption SelectedOption { get; set; }

        public ICollection<AppEntityDataFieldComboboxOption> Options { get; set; }
    }
}