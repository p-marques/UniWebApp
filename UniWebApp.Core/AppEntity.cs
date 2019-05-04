using System.Collections.Generic;

namespace UniWebApp.Core
{
    public class AppEntity
    {
        public int Id { get; set; }
        public AppEntityType Type { get; set; }
        public IList<AppEntityDataField> Fields { get; set; }
    }
}