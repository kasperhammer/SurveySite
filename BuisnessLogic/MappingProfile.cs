using AutoMapper;
using Models.UIModels;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLogic
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapping between Survey and SurveyUI
            CreateMap<Survey, SurveyUI>()
                .ForMember(dest => dest.Comps, opt => opt.MapFrom(src => src.SComps));

            CreateMap<SurveyUI, Survey>()
                .ForMember(dest => dest.SComps, opt => opt.MapFrom(src => src.Comps));

            // Mapping between SComp and CompUI
            CreateMap<SComp, CompUI>()
                .ForMember(dest => dest.MultiAnwsers, opt => opt.MapFrom(src => src.MultiAnwsers))
                .ForMember(dest => dest.SingleAnwser, opt => opt.MapFrom(src => src.SingleAnwser));

            CreateMap<CompUI, SComp>()
                .ForMember(dest => dest.MultiAnwsers, opt => opt.MapFrom(src => src.MultiAnwsers))
                .ForMember(dest => dest.SingleAnwser, opt => opt.MapFrom(src => src.SingleAnwser));

            // Mapping between AnwserModule and AnwserModuleUI
            CreateMap<CompModule, CompModuleUI>().ReverseMap();


            CreateMap<AnwserModuleUI, AnwserModule>().ForMember(x => x.anwsers, o => o.MapFrom(s => s.anwsers));
            CreateMap<AnwserModule, AnwserModuleUI>().ForMember(x => x.anwsers, o => o.MapFrom(s => s.anwsers));

            CreateMap<Anwser, AnwserUI>().ReverseMap();

        }
    }
}
