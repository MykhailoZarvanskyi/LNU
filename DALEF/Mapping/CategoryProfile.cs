using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Concrete;
using DALEF.Models;
using DTO;

namespace DALEF.MappingProfile
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<TblCategory, Category>();
            CreateMap<Category, TblCategory>();
        }
    }
}
