using Application.Dtos;
using Application.General;
using Application.Requests;
using DataAccess.DBContext;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Football.Battle.Server.Controllers
{
    [Authorize]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly Context app;
        private readonly IFootballDBContext context;

        public CategoryController(IFootballDBContext context, IConfiguration configuration)
        {
            this.app = new Context(context, configuration);
        }

        [HttpPost("Categories")]
        public Task<Response<Guid>> CreateCategory([FromBody] CreateCategoryRequest request, [FromHeader(Name = "RoleId")] Guid? roleId)
        {
            return this.app.Services.Category.CreateCategory(request, roleId);
        }

        [HttpPost("Categories/Draft")]
        public Task<Response<Guid>> CreateCategory([FromBody] CreateDraftCategoryRequest request, [FromHeader(Name = "RoleId")] Guid? roleId)
        {
            return this.app.Services.Category.CreateDraftCategory(request, roleId);
        }

        [HttpPut("Categories")]
        public Task<Response<CategoryDTO>> UpdateCategory([FromBody] UpdateCategoryRequest request, [FromHeader(Name = "RoleId")] Guid? roleId)
        {
            return this.app.Services.Category.UpdateCategory(request, roleId);
        }

        [HttpGet("Category/{id}")]
        public Task<Response<CategoryDTO>> GetLanguage([FromRoute] Guid id, [FromHeader(Name = "RoleId")] Guid? roleId)
        {
            return this.app.Services.Category.GetCategory(id, roleId);
        }

        [HttpGet("Categories")]
        public Task<Response<List<CategoryDTO>>> GetLanguages([FromHeader(Name = "RoleId")] Guid? roleId)
        {
            return this.app.Services.Category.GetCategories(roleId);
        }

        [HttpDelete("Category/{id}")]
        public Task<Response> DeleteLanguage([FromRoute] Guid id, [FromHeader(Name = "RoleId")] Guid? roleId)
        {
            return this.app.Services.Category.DeleteCategory(id, roleId);
        }
    }
}
