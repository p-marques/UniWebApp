using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniWebApp.Core
{
    public class AppEntityDataFieldCombobox : AppEntityDataField
    {
        public IList<AppEntityDataFieldComboboxOption> Options { get; set; }
        public AppEntityDataFieldComboboxOption Selected { get; set; }
    }
}
