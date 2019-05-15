using System.ComponentModel.DataAnnotations;

namespace UniWebApp.Core
{
    public class AppEntityDataField
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Section { get; set; }

        // Navigation
        [Required]
        public AppEntity Entity { get; set; }
    }
}