﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniWebApp.Core;

namespace UniWebApp.Web.Models
{
    public class DataFieldTemplateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DataFieldTypeEnum FieldType { get; set; }
        public bool MustHave { get; set; }
        public AppEntityTypeModel EntityType { get; set; }
        public List<DataFieldTemplateComboboxOptionModel> ComboboxOptions { get; set; }
    }

    public class DataFieldTemplateComboboxOptionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class NewDataFieldTemplateModel
    {
        [Required]
        [StringLength(120, MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        public DataFieldTypeEnum FieldType { get; set; }
        [Required]
        public bool MustHave { get; set; }
        [Required]
        public int EntityTypeId { get; set; }
        public string[] ComboboxOptions { get; set; }
    }
}
