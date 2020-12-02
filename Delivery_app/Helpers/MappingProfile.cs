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
            CreateMap<Orders, OrderModel>();
            CreateMap<Users, UserModel>();
            CreateMap<DropPoints, DropPointModel>();
        }
    }
}
