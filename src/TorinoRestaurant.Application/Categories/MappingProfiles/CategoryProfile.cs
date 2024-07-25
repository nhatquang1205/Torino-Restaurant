using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TorinoRestaurant.Application.Categories.Models;
using TorinoRestaurant.Core.Products.Entities;

namespace TorinoRestaurant.Application.Categories.MappingProfiles
{
    public sealed class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryEntity>()
                .ForMember(dest => dest.Id,
                            e => e.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name,
                            e => e.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description,
                            e => e.MapFrom(src => src.Description))
                .ForMember(dest => dest.ImageUrl,
                            e => e.MapFrom(src => src.ImageUrl));
        }
    }
}