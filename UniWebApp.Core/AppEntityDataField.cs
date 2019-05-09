using System.ComponentModel.DataAnnotations;

namespace UniWebApp.Core
{
    public class AppEntityDataField
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public bool Major { get; set; }

        // Navigation
        [Required]
        public AppEntity Entity { get; set; }
    }
}