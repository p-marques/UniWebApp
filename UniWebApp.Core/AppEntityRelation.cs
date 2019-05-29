using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniWebApp.Core
{
    public class AppEntityRelation
    {
        public int Id { get; set; }

        [Required]
        public AppEntity Entity { get; set; }

        [Required]
        public int relatedEntityId { get; set; }

        [Required]
        public string Description { get; set; }

    }
}
