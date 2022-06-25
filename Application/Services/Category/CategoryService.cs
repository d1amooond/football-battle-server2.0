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
    public class CategoryService
    {
        private readonly Context app;
        public CategoryService(Context app)
        {
            this.app = app;
        }

        public async Task<Response<Guid>> CreateDraftCategory(CreateDraftCategoryRequest request, Guid? roleId = null)
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

                var hasPermissions = await this.app.Services.Role.HasPermissions(roleId, (int)acceptedRoles);
                if (!hasPermissions)
                {
                    return response.AccessDenied("Access Denied");
                }

                var languages = await this.app.Repository.Language.GetLanguages();
                if (languages.Where(l => (int)l.Status > (int)LanguageStatuses.Draft).Count() == 0)
                {
                    return response.Failure("There are no languages with status 'In Progress' and above");
                }

                var entity = new Category
                {
                    Name = request.Name,
                    Status = CategoryStatuses.Draft,
                    DisplayNames = languages.Select(language => new CategoryDisplayName
                    {
                        LanguageId = language.Id,
                        LanguageShortCode = language.ShortCode,
                        Name = request.DisplayName,
                    }).ToList()
                };

                var addedCategory = await this.app.Repository.Category.InsertInto<Category>(entity, "categories");
                return response.Success(addedCategory.Id.AsGuid());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return response;
        }

        public async Task<Response<Guid>> CreateCategory(CreateCategoryRequest request, Guid? roleId = null)
        {
            var response = new Response<Guid>();

            var acceptedRoles = Roles.InternalRoles;

            try
            {
                if (string.IsNullOrEmpty(request.Name))
                {
                    return response.Failure("Name is required");
                }

                if (request.DisplayNames.Count() == 0)
                {
                    return response.Failure("Display Names should contain al least one object");
                }

                var hasPermissions = await this.app.Services.Role.HasPermissions(roleId, (int)acceptedRoles);
                if (!hasPermissions)
                {
                    return response.AccessDenied("Access Denied");
                }

                var languages = await this.app.Repository.Language.GetLanguages();

                var proccessLanguages = languages.Where(l => (int)l.Status > (int)LanguageStatuses.Draft);
                if (proccessLanguages.Count() == 0)
                {
                    return response.Failure("There are no languages with status 'In Progress' and above");
                }

                if (!request.DisplayNames.All(n => languages.Any(l => l.ShortCode == n.LanguageShortCode && l.Id == n.LanguageId.AsObjectId())))
                {
                    return response.Failure("Display Names should include all languages");
                }

                var entity = new Category
                {
                    Name = request.Name,
                    Status = CategoryStatuses.InProgress,
                    DisplayNames = request.DisplayNames.Select(n => new CategoryDisplayName
                    {
                        Name = n.Name,
                        LanguageId = n.LanguageId.AsObjectId(),
                        LanguageShortCode = n.LanguageShortCode
                    }).ToList(),
                };

                var createdCategory = await this.app.Repository.Category.InsertInto<Category>(entity, "categories");

                return response.Success(createdCategory.Id.AsGuid());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return response;
        }

        public async Task<Response<CategoryDTO>> UpdateCategory(UpdateCategoryRequest request, Guid? roleId = null)
        {
            var response = new Response<CategoryDTO>();

            var acceptedRoles = Roles.InternalRoles;

            try
            {
                if (string.IsNullOrEmpty(request.Name))
                {
                    return response.Failure("Name is required");
                }

                if (request.DisplayNames.Count() == 0)
                {
                    return response.Failure("Display Names should contain al least one object");
                }

                var hasPermissions = await this.app.Services.Role.HasPermissions(roleId, (int)acceptedRoles);
                if (!hasPermissions)
                {
                    return response.AccessDenied("Access Denied");
                }

                var languages = await this.app.Repository.Language.GetLanguages();

                var proccessLanguages = languages.Where(l => (int)l.Status > (int)LanguageStatuses.Draft);
                if (proccessLanguages.Count() == 0)
                {
                    return response.Failure("There are no languages with status 'In Progress' and above");
                }

                if (!request.DisplayNames.All(n => languages.Any(l => l.ShortCode == n.LanguageShortCode && l.Id == n.LanguageId.AsObjectId())))
                {
                    return response.Failure("Display Names should include all languages");
                }

                var category = await this.app.Repository.Category.GetById<Category>(request.Id.AsObjectId(), "categories");
                if (category == null)
                {
                    return response.NotFound("Category with provided id is not found");
                }

                category.Name = request.Name;
                category.DisplayNames = request.DisplayNames.Select(n => new CategoryDisplayName
                {
                    Name = n.Name,
                    LanguageId = n.LanguageId.AsObjectId(),
                    LanguageShortCode = n.LanguageShortCode
                }).ToList();
                category.Status = request.Status;

                var updatedCategory = await this.app.Repository.Category.Update<Category>(category, "categories");

                return response.Success(updatedCategory.ToDTO());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return response;
        }

        public async Task<Response> DeleteCategory(Guid id, Guid? roleId = null)
        {
            var response = new Response();

            var acceptedRoles = Roles.InternalRoles;

            try
            {
                var hasPermissions = await this.app.Services.Role.HasPermissions(roleId, (int)acceptedRoles);
                if (!hasPermissions)
                {
                    return response.AccessDenied("Access Denied");
                }

                var category = await this.app.Repository.Category.GetById<Category>(id.AsObjectId());
                if (category == null)
                {
                    return response.NotFound("Category with provided id is not found");
                }

                await this.app.Repository.Category.DeleteById(id.AsObjectId());
                return response.Success();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return response;
        }

        public async Task<Response<CategoryDTO>> GetCategory(Guid id, Guid? roleId = null)
        {
            var response = new Response<CategoryDTO>();

            var acceptedRoles = Roles.InternalRoles;

            try
            {
                var hasPermissions = await this.app.Services.Role.HasPermissions(roleId, (int)acceptedRoles);
                if (!hasPermissions)
                {
                    return response.AccessDenied("Access Denied");
                }

                var category = await this.app.Repository.Category.GetById<Category>(id.AsObjectId());
                if (category == null)
                {
                    return response.NotFound("Category with provided id is not found");
                }

                return response.Success(category.ToDTO());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return response;
        }

        public async Task<Response<List<CategoryDTO>>> GetCategories(Guid? roleId = null)
        {
            var response = new Response<List<CategoryDTO>>();

            var acceptedRoles = Roles.InternalRoles;

            try
            {
                var hasPermissions = await this.app.Services.Role.HasPermissions(roleId, (int)acceptedRoles);
                if (!hasPermissions)
                {
                    return response.AccessDenied("Access Denied");
                }

                var categories = await this.app.Repository.Category.GetAll<Category>();
                return response.Success(categories.Select(c => c.ToDTO()).ToList());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return response;
        }
    }
}
