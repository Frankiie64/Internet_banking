﻿using AutoMapper;
using Internet_banking.Core.Application.Dtos.Account;
using Internet_banking.Core.Application.ViewModels.Products;
using Internet_banking.Core.Application.ViewModels.TypeAccount;
using Internet_banking.Core.Application.ViewModels.Users;
using Internet_banking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<AuthenticationRequest, LoginVM>()
                .ForMember(x => x.HasError, opt=> opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<RegisterRequest, SaveUserVM>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ForgotPassowordRequest, ForgotPasswordVM>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ResetPasswordRequest, ResetPasswordVM>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<AuthenticationResponse, UserVM>()               
               .ReverseMap()
               .ForMember(x => x.HasError, opt => opt.Ignore())
               .ForMember(x => x.Error, opt => opt.Ignore());

            CreateMap<TypeAccount, TypeAccountVM>()
              .ReverseMap()
              .ForMember(x => x.LastModified, opt => opt.Ignore())
              .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
              .ForMember(x => x.Creadted, opt => opt.Ignore())
              .ForMember(x => x.CreatedBy, opt => opt.Ignore());

            CreateMap<TypeAccount, SaveTypeAccountVM>()
              .ReverseMap()
              .ForMember(x => x.LastModified, opt => opt.Ignore())
              .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
              .ForMember(x => x.Creadted, opt => opt.Ignore())
              .ForMember(x => x.CreatedBy, opt => opt.Ignore())
              .ForMember(x => x.Products, opt => opt.Ignore());

            CreateMap<TypeAccount, SaveTypeAccountVM>()
             .ReverseMap()
             .ForMember(x => x.Products, opt => opt.Ignore());

            CreateMap<Products, ProductsVM>()
              .ForMember(x => x.client, opt => opt.Ignore())
              .ReverseMap()
              .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
              .ForMember(x => x.Creadted, opt => opt.Ignore())
              .ForMember(x => x.CreatedBy, opt => opt.Ignore())
              .ForMember(x => x.LastModified, opt => opt.Ignore());

            CreateMap<Products, SaveProductVM>()
              .ReverseMap()
              .ForMember(x => x.TypeAccount, opt => opt.Ignore())
              .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
              .ForMember(x => x.Creadted, opt => opt.Ignore())
              .ForMember(x => x.CreatedBy, opt => opt.Ignore())
              .ForMember(x => x.LastModified, opt => opt.Ignore());

            CreateMap<ProductsVM, SaveProductVM>()
            .ReverseMap()
            .ForMember(x => x.TypeAccount, opt => opt.Ignore())
            .ForMember(x => x.client, opt => opt.Ignore());
        }
    }
}
