using AutoMapper;
using Domain.Models.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            /*1*/
            CreateMap<RegisterStudentDto, Students>().
            ForMember(dest => dest.Status, src => src.MapFrom(opt => opt.Status.ToString())).
            ForMember(dest => dest.Gender, src => src.MapFrom(opt => opt.gender.ToString()));


            /*2*/
            CreateMap<Students, RegisterStudentDto>().
             ForMember(dest => dest.Status, src => src.MapFrom(opt => Enum.Parse<StudentState>(opt.Status)))
            .ForMember(dest => dest.gender, src => src.MapFrom(opt => Enum.Parse<Gender>(opt.Gender)));


            /*3*/
            CreateMap<RegisterTeacherDto, Teacher>().
           ForMember(dest => dest.Status, src => src.MapFrom(opt => opt.Status.ToString()))
          .ForMember(dest => dest.Gender, src => src.MapFrom(opt => opt.gender.ToString()));


            /*4*/
            CreateMap<Teacher, RegisterTeacherDto>().
            ForMember(dest => dest.Status, src => src.MapFrom(opt => Enum.Parse<TeacherState>(opt.Status)))
           .ForMember(dest => dest.gender, src => src.MapFrom(opt => Enum.Parse<Gender>(opt.Gender)));

            /*5*/

            CreateMap<Teacher, UserProfileDto>()
               .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom<PictureUrlResolver>());
            /*6*/
            CreateMap<Students, UserProfileDto>()
               .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom<PictureUrlResolver>());
        }

    }
    //For Resolving the Full URL of the Picture
    public class PictureUrlResolver(IConfiguration config) : IValueResolver<AppUsers, UserProfileDto, string>
    {
        public string Resolve(AppUsers source, UserProfileDto destination, string destMember, ResolutionContext context)
        {
            var baseUrl = config["BaseUrl"] ?? string.Empty;

            if (string.IsNullOrWhiteSpace(baseUrl) || source.ProfileImage == null || !source.ProfileImage.Any())
                return string.Empty;


            if (string.IsNullOrWhiteSpace(source.ProfileImage))
                return $"{baseUrl}/images/default-profile.png";

            // I make it to Avoid Double Slash in the URL
            return $"{baseUrl.TrimEnd('/')}/{source.ProfileImage.TrimStart('/')}";

        }
    }
}
