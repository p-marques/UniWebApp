using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniWebApp.Core
{
    public class AppEntity
    {
        public int Id { get; set; }

        [Required]
        public AppEntityType Type { get; set; }

        public ICollection<AppEntityDataField> Fields { get; set; }
    }
}