using AutoMapper;
using Raffle.Aplication.DTOs.RaffleDto;
using Raffle.Domain.Entities.Raffle;

namespace Raffle.Application.Mappings
{
    public class RaffleMappingProfile : Profile
    {
        public RaffleMappingProfile()
        {
            // CreateRaffleDtoRequest -> RaffleEntity
            CreateMap<CreateRaffleDtoRequest, RaffleEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            // UpdateRaffleDtoRequest -> RaffleEntity
            CreateMap<UpdateRaffleDtoRequest, RaffleEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // RaffleEntity -> RaffleDto
            CreateMap<RaffleEntity, RaffleDto>();
            
            // RaffleEntity -> CreateRaffleDtoResponse
            CreateMap<RaffleEntity, CreateRaffleDtoResponse>();
        }
    }
}