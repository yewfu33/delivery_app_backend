using AutoMapper;
using Delivery_app.Entities;
using Delivery_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Orders, OrderModel>()
                .ForMember(om => om.delivery_status,
                    o => o.MapFrom(y => (int?)y.delivery_status))
                .ForMember(om => om.vehicle_type,
                    o => o.MapFrom(y => (int?)y.vehicle_type));
            CreateMap<Users, UserModel>()
                .ForMember(um => um.user_type,
                    u => u.MapFrom(y => (int?)y.user_type));
            CreateMap<Couriers, CourierModel>();
            CreateMap<DropPoints, DropPointModel>();
            CreateMap<AddPaymentModel, Payments>();
        }
    }
}
