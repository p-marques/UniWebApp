using System.Collections.Generic;

namespace UniWebApp.Core
{
    public class AppEntityType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation
        public ICollection<AppEntity> Entities { get; set; }

        public ICollection<DataFieldTemplate> TemplateFields { get; set; }
    }
}