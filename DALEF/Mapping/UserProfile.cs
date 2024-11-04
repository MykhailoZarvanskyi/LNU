using AutoMapper;
using DALEF.Models;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALEF.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<TblUser, User>();
            CreateMap<User, TblUser>();
        }
    }
}
