using Internet_banking.Core.Application.Dtos.Account;
using Internet_banking.Core.Application.Dtos.Email;
using Internet_banking.Core.Application.Enums;
using Internet_banking.Core.Application.Interfaces.Services;
using Internet_banking.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Infrastructure.Identity.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signManager;
        private readonly IEmailService emailService;

        public AccountServices(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signManager, IEmailService emailService)
        {
            this.userManager = userManager;
            this.signManager = signManager;
            this.emailService = emailService;
        }

        public async Task<AuthenticationResponse> AuthenticationAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new();


            var user = await userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"La cuenta {request.Email} no se encuentra registrada.";
                return response;
            }

            var result = await signManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"La credenciales son incorrectas.";
                return response;
            }

            if (!user.IsActive)
            {
                response.HasError = true;
                response.Error = $"Su cuenta esta inactiva.";
                return response;
            }

            response.Id = user.Id;
            response.Email = user.Email;
            response.Username = user.UserName;

            var rolesList = await userManager.GetRolesAsync(user).ConfigureAwait(false);

            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;

            return response;
        }

        public async Task SignOutAsync()
        {
            await signManager.SignOutAsync();
        }
        public async Task<RegisterResponse> RegisterUserAsync(RegisterRequest request, string origin)
        {
            RegisterResponse response = new();
            response.HasError = false;

            var userWithUsername = await userManager.FindByNameAsync(request.Username);

            if (userWithUsername != null)
            {
                response.HasError = true;
                response.Error = $"El nombre de usuario '{request.Username}' existe.";
                return response;
            }

            var userWithEmail = await userManager.FindByEmailAsync(request.Email);

            if (userWithEmail != null)
            {
                response.HasError = true;
                response.Error = $"El email '{request.Email}' fue registrado.";
                return response;
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                Firstname = request.Firstname,
                Lastname = request.Lastname,
                DocumementId = request.DocumementId,
                UserName = request.Username,     
                PhoneNumber = request.PhoneNumber,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, request.Rol);
            }
            else
            {
                response.HasError = true;
                response.Error = $"Utilice una contraseña mas segura";
                return response;
            }

            var item = await userManager.FindByEmailAsync(user.Email);

            response.IdClient = item.Id;

            return response;
        }
        public async Task<string> ConfirmEmailAsync(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return "No se ha registrado ningun usuario con este correo.";
            }

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return $"La cuenta ha sido confirmada para el correo de {user.Email}. Ahora puede utilizar nuestra aplicacion.";
            }
            else
            {
                return $"Ha ocurrido un error tratando de validar el correo {user.Email}.";
            }
        }

        public async Task<ForgotPassWordResponse> ForgotPasswordRequestAsync(ForgotPassowordRequest request, string origin)
        {
            ForgotPassWordResponse response = new();
            response.HasError = false;

            var account = await userManager.FindByEmailAsync(request.Email);

            if (account == null)
            {
                response.HasError = true;
                response.Error = $"La cuenta {request.Email} no se encuentra registrada.";
                return response;
            }
            var url = await SendForgotPasswordVerificationEmailUrl(account, origin);

            await emailService.SendAsync(new EmailRequest()
            {
                To = request.Email,
                Body = $"Por favor confirma tu cuenta, mediante la visita de este link. {url}",
                Subject = "Recuperar contraseña"
            });


            return response;
        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            ResetPasswordResponse response = new();
            response.HasError = false;

            var account = await userManager.FindByEmailAsync(request.Email);

            if (account == null)
            {
                response.HasError = true;
                response.Error = $"La cuenta {request.Email} no se encuentra registrada.";
                return response;
            }

            request.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));

            var result = await userManager.ResetPasswordAsync(account, request.Token, request.Password);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Ha ocurrido un error reseteando la contraseña";
                return response;

            }
            return response;
        }


        private async Task<string> SendVerificationEmailUrl(ApplicationUser user, string origin)
        {
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var route = "User/ConfirmEmail";

            var url = new Uri(string.Concat($"{origin}/", route));
            var verificationUrl = QueryHelpers.AddQueryString(url.ToString(), "userId", user.Id);
            verificationUrl = QueryHelpers.AddQueryString(verificationUrl, "token", code);

            return verificationUrl;
        }
        private async Task<string> SendForgotPasswordVerificationEmailUrl(ApplicationUser user, string origin)
        {
            var code = await userManager.GeneratePasswordResetTokenAsync(user);

            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var route = "User/ResetPassword";

            var url = new Uri(string.Concat($"{origin}/", route));
            var verificationUrl = QueryHelpers.AddQueryString(url.ToString(), "token", code);

            return verificationUrl;
        }

        public async Task<List<AuthenticationResponse>> GetAllUsersAsync()
        {
            var items = await userManager.Users.ToListAsync();

            

            List<AuthenticationResponse> list = new();

            foreach (var vm in items)
            {
                var rol = await userManager.GetRolesAsync(vm).ConfigureAwait(false);

                AuthenticationResponse item = new AuthenticationResponse
                {
                    Id = vm.Id,
                    Firstname = vm.Firstname,
                    Lastname = vm.Lastname,
                    DocumementId = vm.DocumementId,
                    Username = vm.UserName,
                    Email = vm.Email,
                    PhoneNumber = vm.PhoneNumber,
                    Roles = rol.ToList(),
                    IsVerified = vm.IsActive
                };

                if(item.Roles.Count() == 1)
                {
                    if (item.Roles[0] == "Basic")
                    {
                        item.Roles.Clear();
                        item.Roles.Add("Cliente");
                    }                  
                }

                list.Add(item);
            };

            return list;
        }
        public async Task<AuthenticationResponse> GetUserByIdAsync(string id)
        {
            var vm = await userManager.FindByIdAsync(id);

            var rol = await userManager.GetRolesAsync(vm).ConfigureAwait(false);

            AuthenticationResponse item = new AuthenticationResponse
            {
                Id = vm.Id,
                Username = vm.UserName,
                Email = vm.Email,
                DocumementId = vm.DocumementId,
                Firstname = vm.Firstname,
                Lastname = vm.Lastname,
                PhoneNumber = vm.PhoneNumber,
                Roles = rol.ToList(),
                IsVerified = vm.IsActive
            };

            return item;
        }

        public async Task<AuthenticationResponse> IsVerified(string id)
        {
            var vm = await userManager.FindByIdAsync(id);

            var rol = await userManager.GetRolesAsync(vm).ConfigureAwait(false);

            if (vm.EmailConfirmed == false)
            {
                string token = await userManager.GenerateEmailConfirmationTokenAsync(vm);
                var response = await userManager.ConfirmEmailAsync(vm, token);
            }

            vm.IsActive = vm.IsActive == true ? false : true;

            await userManager.UpdateAsync(vm);

            AuthenticationResponse item = new AuthenticationResponse
            {
                Id = vm.Id,
                Username = vm.UserName,
                Email = vm.Email,
                Roles = rol.ToList(),
                IsVerified = vm.IsActive
            };

            return item;
        }
        public async Task<RegisterResponse> UpdateUserAsync(RegisterRequest request)
        {
            RegisterResponse response = new();
            response.HasError = false;

            var user = await userManager.FindByIdAsync(request.Id);

            var userWithUsername = await userManager.FindByNameAsync(request.Username);

            if (userWithUsername != null && user.UserName != request.Username)
            {
                response.HasError = true;
                response.Error = $"El nombre de usuario '{request.Username}' ya  existe.";
                return response;
            }

            var userWithEmail = await userManager.FindByEmailAsync(request.Email);

            if (userWithEmail != null && user.Email != request.Email)
            {
                response.HasError = true;
                response.Error = $"El email '{request.Email}' ya esta en uso.";
                return response;
            }


            
                user.Email = request.Email;
                user.Firstname = request.Firstname;
                user.Lastname = request.Lastname;
                user.DocumementId = request.DocumementId;
                user.UserName = request.Username;
                user.PhoneNumber = request.PhoneNumber;
                user.EmailConfirmed = true;
                user.PhoneNumberConfirmed = true;                

            string error = string.Empty;
            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded && result.Errors.Count() == 1)
            {
                error = result.Errors.FirstOrDefault(eror => eror.Code == "DuplicateUserName").Code;
            }

            

            if (!string.IsNullOrWhiteSpace(error) || result.Succeeded)
            {
                await userManager.RemovePasswordAsync(user);

                result = await userManager.AddPasswordAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    response.HasError = true;
                    response.Error = $"Utilice una contraseña mas segura";
                    return response;
                }
            }
            else
            {
                response.HasError = true;
                response.Error = $"Ha ocurrido un error creando el usuario";
                return response;
            }

            var item = await userManager.FindByEmailAsync(user.Email);

            response.IdClient = item.Id;

            return response;
        }
    }
}
