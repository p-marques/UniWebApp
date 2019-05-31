using System.Collections.Generic;

namespace UniWebApp.Web.Models
{
    public class AppEntityModel
    {
        public int Id { get; set; }

        public int TypeId { get; set; }

        public string TypeName { get; set; }

        public string Name { get; set; }

        public List<AppEntityDataFieldModel> Fields { get; set; }

        public List<AppEntityRelationModel> Relations { get; set; }
    }

    public class NewAppEntityModel
    {
        public int TypeId { get; set; }

        public List<NewAppEntityDataFieldModel> Fields { get; set; }

        public List<AppEntityRelationModel> Relations { get; set; }
    }
}