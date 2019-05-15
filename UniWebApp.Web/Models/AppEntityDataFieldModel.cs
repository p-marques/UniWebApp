using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UniWebApp.Core;

namespace UniWebApp.Web.Models
{
    public class AppEntityDataFieldModel
    {
        public int FieldId { get; set; }

        public string Name { get; set; }

        public DataFieldTypeEnum FieldType { get; set; }

        public bool BooleanValue { get; set; }

        public List<string> ComboboxOptions { get; set; }

        public int ComboboxSelected { get; set; }

        public DateTime DateValue { get; set; }

        public decimal NumberValue { get; set; }

        public string TextValue { get; set; }

        public string Section { get; set; }
    }

    public class NewAppEntityDataFieldModel
    {
        public string Name { get; set; }

        public int EntityId { get; set; }

        public DataFieldTypeEnum FieldType { get; set; }

        public bool BooleanValue { get; set; }

        public string[] ComboboxOptions { get; set; }

        public int ComboboxSelected { get; set; }

        public DateTime DateValue { get; set; }

        public decimal NumberValue { get; set; }

        public string TextValue { get; set; }

        public string Section { get; set; }
    }
}