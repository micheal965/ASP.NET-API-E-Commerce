using AutoMapper;
using Talabat.APIs.DTOs.Basket;
using Talabat.APIs.DTOs.Order;
using Talabat.APIs.DTOs.Product;
using Talabat.Core.Entities;
using UserAddress = Talabat.Core.Entities.Identity.Address;
using UserAddressDto = Talabat.APIs.DTOs.Account.UserAddressDto;

using OrderAddress = Talabat.Core.Entities.Order_Aggregate.Address;
using OrderAddressDto = Talabat.APIs.DTOs.Order.AddressDto;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.APIs.DTOs.Account;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(p => p.BrandName, o => o.MapFrom(o => o.Brand.Name))
                .ForMember(p => p.CategoryName, o => o.MapFrom(o => o.Category.Name))
                .ForMember(p => p.PictureUrl, o => o.MapFrom<ProductPictureUrlReslover>());

            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketProducts>();
            //Order Address
            CreateMap<OrderAddressDto, OrderAddress>();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(od => od.DeliveryMethodName, o => o.MapFrom(o => o.DeliveryMethod.ShortName))
                .ForMember(od => od.DeliverMethodCost, o => o.MapFrom(o => o.DeliveryMethod.Cost));
            CreateMap<OrderItem, OrderItemDto>();
            //.ForMember(od => od.Quantity, oi => oi.MapFrom(oi => oi.Quantity))
            //.ForMember(od => od.Price, oi => oi.MapFrom(oi => oi.Price));

            //UserAddress
            CreateMap<UserAddress, UserAddressDto>().ReverseMap();

        }
    }
}
