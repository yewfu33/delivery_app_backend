using AutoMapper;
using Delivery_app.Entities;
using Delivery_app.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.web.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Orders, OrderViewModel>();
            CreateMap<Users, UserViewModel>().ReverseMap();
            CreateMap<PromoCodes, PromoCodeModel>().ReverseMap();
            CreateMap<Couriers, CourierViewModel>().ReverseMap();
        }
    }
}
