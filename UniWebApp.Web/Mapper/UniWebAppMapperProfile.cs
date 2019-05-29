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
        }
    }
}