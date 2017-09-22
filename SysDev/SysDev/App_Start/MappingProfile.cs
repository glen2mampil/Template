using System;
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
            Mapper.CreateMap<MasterData, MasterDataDto>();
            Mapper.CreateMap<MasterDetail, MasterDetailDto>();
            //Mapper.CreateMap<ApplicationUser, AccountDto>();


            // Dto to Domain
            Mapper.CreateMap<AuditTrailDto, AuditTrail>()
                .ForMember(ad => ad.Id, opt => opt.Ignore());
            Mapper.CreateMap<UserProfileDto, UserProfile>()
                .ForMember(ad => ad.Id, opt => opt.Ignore());
            Mapper.CreateMap<MasterDataDto, MasterData>()
                .ForMember(ad => ad.Id, opt => opt.Ignore());
            Mapper.CreateMap<MasterDetailDto, MasterDetail>()
                .ForMember(ad => ad.Id, opt => opt.Ignore());
            //Mapper.CreateMap<AccountDto, ApplicationUser>();
        }
    }
}