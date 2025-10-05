using AutoMapper;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Shared.Lesson_Dto;
using Shared.SubjectDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Profiles
{
    public  class LessonProfile:Profile
    {
        public LessonProfile()
        {
            CreateMap<CreateLessonDto, Lesson>()
                .ForAllMembers(opt=>opt.Condition((src,dest,srcMember)=>srcMember !=null));

            CreateMap<UpdateLessonDto, Lesson>()
                .ForMember(dest=>dest.Title,opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Lesson, LessonDetailsResponseDto>()
                .ForMember(dest=>dest.MaterialUrl,src=>src.MapFrom<MatrialResolver>());
        }
       
    }
    public class MatrialResolver(IConfiguration _config) : IValueResolver<Lesson, LessonDetailsResponseDto, string>
    {
        public string Resolve(Lesson source, LessonDetailsResponseDto destination, string destMember, ResolutionContext context)
        {

            var result = string.IsNullOrEmpty(source.MaterialUrl) ? string.Empty : $"{_config["BaseUrl"].TrimEnd('/')}/{source.MaterialUrl.TrimStart('/')}";
            return result;
        }
    }
}
