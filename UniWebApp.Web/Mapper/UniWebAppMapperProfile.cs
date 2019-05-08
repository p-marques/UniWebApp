using AutoMapper;
using UniWebApp.Core;
using UniWebApp.Web.Models;

namespace UniWebApp.Web.Mapper
{
    public class UniWebAppMapperProfile : Profile
    {
        public UniWebAppMapperProfile()
        {
            this.CreateMap<AppEntityType, AppEntityTypeModel>().ReverseMap();
            this.CreateMap<NewDataFieldTemplateModel, DataFieldTemplate>().ForMember(z => z.EntityType, opt => opt.Ignore()).ForMember(c => c.ComboboxOptions, rt => rt.Ignore());
            this.CreateMap<DataFieldTemplate, DataFieldTemplateModel>();
            this.CreateMap<DataFieldTemplateComboboxOption, DataFieldTemplateComboboxOptionModel>();
        }
    }
}