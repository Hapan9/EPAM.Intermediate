using AutoMapper;
using EPAM.Persistence.Entities;
using EPAM.Services.Dtos;
using EPAM.Services.Dtos.Order;

namespace EPAM.Services.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Venue, VenueDto>()
                .ReverseMap();

            CreateMap<Section, SectionDto>()
                .ReverseMap();

            CreateMap<PriceOption, PriceOptionDto>()
                .ReverseMap();

            CreateMap<Event, EventDto>()
                .ReverseMap();

            CreateMap<SeatStatus, SeatStatusDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<Seat, SeatDto>()
                .ForMember(dest => dest.PriceOptionDto, opt => opt.Ignore())
                .ForMember(dest => dest.SeatStatusDto, opt => opt.Ignore());

            CreateMap<Order, GetOrderDto>()
                .ReverseMap();

            CreateMap<Order, CreateOrderDto>()
                .ReverseMap();

            CreateMap<Payment, PaymentDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}
