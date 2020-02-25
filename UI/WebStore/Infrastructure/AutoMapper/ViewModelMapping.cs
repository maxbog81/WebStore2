using AutoMapper;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewModels;
using WebStore.Domain.ViewModels.Identity;

namespace WebStore.Infrastructure.AutoMapper
{
    public class ViewModelMapping : Profile
    {
        public ViewModelMapping()
        {
            CreateMap<RegisterUserViewModel, User>()
               //.ForMember(UserModel => UserModel.UserName, opt => opt.MapFrom(ViewModel => ViewModel.UserName))
                ;

        }
    }
}
