﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using SysDev.Dtos;
using SysDev.Models;

namespace SysDev.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to Dto
            Mapper.CreateMap<AuditTrail, AuditTrailDto>();
            Mapper.CreateMap<UserProfile, UserProfileDto>();


            // Dto to Domain
            Mapper.CreateMap<AuditTrailDto, AuditTrail>()
                .ForMember(ad => ad.Id, opt => opt.Ignore());
            Mapper.CreateMap<UserProfileDto, UserProfile>()
                .ForMember(ad => ad.Id, opt => opt.Ignore());

        }
    }
}