using AutoMapper;
using CDS_API.Domain.Entities;
using CDS_API.Application.DTOs;

namespace CDS_API.Application.Mappings
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            // Map from Domain Entity to DTO
            CreateMap<Course, CourseDto>()
                .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CourseCode, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.EffectiveDate, opt => opt.MapFrom(src => src.EffectiveDate))
                .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.ExpiryDate));

            // Optionally, map to GetCoursesResponse if needed
            CreateMap<Course, GetCoursesResponse>()
                .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CourseCode, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.EffectiveDate, opt => opt.MapFrom(src => src.EffectiveDate))
                .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.ExpiryDate));
        }
    }
}