using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniWebApp.Web.Models
{
    public enum DataFieldTypeEnum
    {
        Text = 0,
        Number,
        Date,
        Combobox,
        Boolean
    }
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
