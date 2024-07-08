using AutoMapper;
using CompanyManagementSystem.DAL.Models;
using CompanyManagementSystem.PL.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace CompanyManagementSystem.PL.AutoMapper
{
    public class OrganizationProfile : Profile
    {

        public OrganizationProfile()
        {
            // employee Profile
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();

            // User Profile
            CreateMap<ApplicationUser, UserViewModel>()
          .ForMember(dest => dest.Roles, opt => opt.Ignore()).ReverseMap();

            // role Profile
            CreateMap<RoleViewModel, IdentityRole>()
           .ForMember(dest => dest.NormalizedName, opt => opt.MapFrom(src => src.Name.ToUpper()))
           .ReverseMap();
            
        }
    }
}
