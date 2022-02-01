using AutoMapper;

namespace Blog.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PostViewModel, Post>().ReverseMap()
                .ForMember(dest => dest.Image, opt => opt.Ignore());
            CreateMap<AuthUserViewModel, IdentityUser>().ReverseMap();
            CreateMap<CommentViewModel, MainComment>().ReverseMap();
        }
    }
}
