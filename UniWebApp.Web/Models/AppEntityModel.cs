using System.Collections.Generic;

namespace UniWebApp.Web.Models
{
    public class AppEntityModel
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
    }

    public class NewAppEntityModel
    {
        public int TypeId { get; set; }

        public List<NewAppEntityDataFieldModel> Fields { get; set; }
    }
}