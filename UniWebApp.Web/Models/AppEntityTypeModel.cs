using System.ComponentModel.DataAnnotations;

namespace UniWebApp.Web.Models
{
    public class AppEntityTypeModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}