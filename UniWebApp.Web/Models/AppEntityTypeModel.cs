using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniWebApp.Web.Models
{
    public class AppEntityTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<DataFieldTemplateModel> TemplateFields { get; set; }
    }

    public class NewAppEntityTypeModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<NewDataFieldTemplateModel> TemplateFields { get; set; }
    }
}