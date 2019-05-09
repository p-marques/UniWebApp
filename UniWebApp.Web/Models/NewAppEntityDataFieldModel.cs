using System.ComponentModel.DataAnnotations;
using UniWebApp.Core;

namespace UniWebApp.Web.Models
{
    public class NewAppEntityDataFieldModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(120, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        public bool Major { get; set; }

        [Required]
        public int EntityId { get; set; }

        [Required]
        public bool AddToAllEntitiesOfType { get; set; }

        [Required]
        public DataFieldTypeEnum FieldType { get; set; }
    }
}