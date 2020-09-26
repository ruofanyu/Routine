using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Routine.API.Entities;
using Routine.API.Models;

namespace Routine.API.Profiles
{
    //配置映射关系的函数
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            //创建一个映射，该映射关系是CompanyDto是目标，Company是源
            CreateMap<Company, CompanyDto>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            CreateMap<CompanyAddDto, Company>();
        }
    }
}
