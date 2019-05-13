using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniWebApp.Core
{
    public class DataFieldTemplate
    {
        public int Id { get; set; }

        [Required]
        [StringLength(120, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        public DataFieldTypeEnum FieldType { get; set; }

        [Required]
        public AppEntityType EntityType { get; set; }

        public ICollection<DataFieldTemplateComboboxOption> ComboboxOptions { get; set; }
    }
}