using AutoMapper;
using Domain.Models.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared.IdentityDtos;
using Shared.IdentityDtos.Admin;
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
            CreateMap<CreateStudentDto, Students>().
            ForMember(dest => dest.Status, src => src.MapFrom(opt => opt.Status.ToString())).
            ForMember(dest => dest.Gender, src => src.MapFrom(opt => opt.gender.ToString()));

            CreateMap<UpdateStudentDto, Students>().
            ForMember(dest => dest.Status, src => src.MapFrom(opt => opt.Status.ToString())).
            ForMember(dest => dest.Gender, src => src.MapFrom(opt => opt.gender.ToString()));


            /*2*/
            CreateMap<Students, CreateStudentDto>().
             ForMember(dest => dest.Status, src => src.MapFrom(opt => Enum.Parse<StudentState>(opt.Status)))
            .ForMember(dest => dest.gender, src => src.MapFrom(opt => Enum.Parse<Gender>(opt.Gender)));
            
            CreateMap<Students, UpdateStudentDto>().
             ForMember(dest => dest.Status, src => src.MapFrom(opt => Enum.Parse<StudentState>(opt.Status)))
            .ForMember(dest => dest.gender, src => src.MapFrom(opt => Enum.Parse<Gender>(opt.Gender)));


            /*3*/
            CreateMap<CreateTeacherDto, Teacher>().
           ForMember(dest => dest.Status, src => src.MapFrom(opt => opt.Status.ToString()))
          .ForMember(dest => dest.Gender, src => src.MapFrom(opt => opt.gender.ToString()));


            /*4*/
            CreateMap<Teacher, CreateTeacherDto>().
            ForMember(dest => dest.Status, src => src.MapFrom(opt => Enum.Parse<UserState>(opt.Status)))
           .ForMember(dest => dest.gender, src => src.MapFrom(opt => Enum.Parse<Gender>(opt.Gender)));



            /*5*/
            CreateMap<Teacher, UserProfileDto>()
               .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom<PictureUrlResolver>());
            /*6*/
            CreateMap<Students, UserProfileDto>()
               .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom<PictureUrlResolver>());
            /*7*/
            CreateMap<UpdateTeacherDto, Teacher>()
           .ForMember(dest => dest.Status, src => src.MapFrom(opt => opt.Status.ToString()));

            CreateMap<Teacher, TeacherResultDto>().
                ForMember(dest => dest.ProfileImage, src => src.MapFrom<PictureUrlResolverResutlTeacher>());

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
    //For TEACHER Result
    public class PictureUrlResolverResutlTeacher(IConfiguration config) : IValueResolver<Teacher, TeacherResultDto, string>
    {
        public string Resolve(Teacher source, TeacherResultDto destination, string destMember, ResolutionContext context)
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
