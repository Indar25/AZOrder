using AutoMapper;
using Order_Application.Command;
using Order_Domain.Domain;

namespace Order_Application;
public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<CreateOrderCommand, Order>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
