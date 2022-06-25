using Application.Dtos;
using Application.Extensions;
using Application.General;
using Application.Requests;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils.Extensions;

namespace Application.Services
{
    public class LanguageService
    {
        private readonly Context app;
        public LanguageService(Context app)
        {
            this.app = app;
        }

        public async Task<Response<Guid>> CreateLanguage(CreateLanguageRequest request, Guid? roleId = null)
        {
            var response = new Response<Guid>();

            var acceptedRoles = Roles.InternalRoles;

            try
            {
                if (string.IsNullOrEmpty(request.Name))
                {
                    return response.Failure("Name is required");
                }

                if (string.IsNullOrEmpty(request.DisplayName))
                {
                    return response.Failure("Display Name is required");
                }

                if (string.IsNullOrEmpty(request.ShortCode))
                {
                    return response.Failure("Short code is required");
                }

                var hasPermissions = await this.app.Services.Role.HasPermissions(roleId, (int)acceptedRoles);
                if (!hasPermissions)
                {
                    return response.AccessDenied("Access Denied");
                }


                var language = await this.app.Repository.Language.GetByShortCode(request.ShortCode);
                if (language != null)
                {
                    return response.Failure("Language with current short code already exists");
                }


                var entity = new Language
                {
                    Name = request.Name,
                    Status = LanguageStatuses.Draft,
                    ShortCode = request.ShortCode
                };

                var createdLanguage = await this.app.Repository.Language.InsertInto<Language>(entity, "languages");

                return response.Success(createdLanguage.Id.AsGuid());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return response;
        }

        public async Task<Response<LanguageDTO>> UpdateLanguage(UpdateLanguageRequest request, Guid? roleId)
        {
            var response = new Response<LanguageDTO>();

            var acceptedRoles = Roles.InternalRoles;

            try
            {
                if (string.IsNullOrEmpty(request.Name))
                {
                    return response.Failure("Name is required");
                }

                if (string.IsNullOrEmpty(request.DisplayName))
                {
                    return response.Failure("Display Name is required");
                }

                if (string.IsNullOrEmpty(request.ShortCode))
                {
                    return response.Failure("Short code is required");
                }

                var hasPermissions = await this.app.Services.Role.HasPermissions(roleId, (int)acceptedRoles);
                if (!hasPermissions)
                {
                    return response.AccessDenied("Access Denied");
                }

                var language = await this.app.Repository.Language.GetById<Language>(request.Id.AsObjectId(), "languages");
                if (language == null)
                {
                    return response.Failure("Language is not found");
                }

                language.Name = request.Name;
                language.Status = request.Status;

                var updatedEntity = await this.app.Repository.Language.Update<Language>(language, "languages");

                return response.Success(updatedEntity.ToDTO());


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return response;
        }

        public async Task<Response<LanguageDTO>> GetLanguage(Guid languageId, Guid? roleId)
        {
            var response = new Response<LanguageDTO>();

            var acceptedRoles = Roles.InternalRoles;

            try
            {
                var hasPermissions = await this.app.Services.Role.HasPermissions(roleId, (int)acceptedRoles);
                if (!hasPermissions)
                {
                    return response.Failure("Access Denied");
                }

                var language = await this.app.Repository.Language.GetById<Language>(languageId.AsObjectId(), "languages");
                if (language == null)
                {
                    return response.Failure("Language is not found");
                }

                return response.Success(language.ToDTO());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return response;
        }

        public async Task<Response<List<LanguageDTO>>> GetLanguages(Guid? roleId)
        {
            var response = new Response<List<LanguageDTO>>();

            var acceptedRoles = Roles.InternalRoles;

            try
            {
                var hasPermissions = await this.app.Services.Role.HasPermissions(roleId, (int)acceptedRoles);
                if (!hasPermissions)
                {
                    return response.AccessDenied("Access Denied");
                }

                var languages = await this.app.Repository.Language.GetLanguages();

                return response.Success(languages.Select(language => language.ToDTO()).ToList());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return response;
        }

        public async Task<Response> DeleteLanguage(Guid languageId, Guid? roleId)
        {
            var response = new Response();

            var acceptedRoles = Roles.InternalRoles;

            try
            {
                var hasPermissions = await this.app.Services.Role.HasPermissions(roleId, (int)acceptedRoles);
                if (!hasPermissions)
                {
                    return response.Failure("Access Denied");
                }

                var language = await this.app.Repository.Language.GetById<Language>(languageId.AsObjectId(), "languages");
                if (language == null)
                {
                    return response.Failure("Language is not found");
                }

                await this.app.Repository.Language.DeleteById(language.Id);

                return response.Success();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return response;
        }
    }
}
