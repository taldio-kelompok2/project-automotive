using AutoMapper;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.WebAPI.Dto.Courses;

namespace AutomotiveApp.WebAPI.Mapper
{
    public class CourseCategoryRequestProfile : Profile
    {
        public CourseCategoryRequestProfile()
        {
            CreateMap<CourseCategoryCreateRequest, CourseCategoryCommandDto>()
            .ForMember(
                dest => dest.ImageFileName,
                opt => opt.MapFrom(src => src.Image != null ? src.Image.FileName : null)
            );

            CreateMap<CourseCategoryEditRequest, CourseCategoryEditDto>()
            .ForAllMembers(opt =>
            {
                opt.Condition((src, dest, srcMember) => srcMember != null);
            });
        }
    }
}