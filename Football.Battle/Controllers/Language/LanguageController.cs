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
    //[Authorize]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly Context app;
        private readonly IFootballDBContext context;

        public LanguageController(IFootballDBContext context, IConfiguration configuration)
        {
            this.app = new Context(context, configuration);
        }

        [HttpPost("Languages")]
        public Task<Response<Guid>> CreateLanguage([FromBody] CreateLanguageRequest request, [FromHeader(Name = "RoleId")] Guid? roleId)
        {
            return this.app.Services.Language.CreateLanguage(request, roleId);
        }

        [HttpPut("Languages")]
        public Task<Response<LanguageDTO>> UpdateLanguage([FromBody] UpdateLanguageRequest request, [FromHeader(Name = "RoleId")] Guid? roleId)
        {
            return this.app.Services.Language.UpdateLanguage(request, roleId);
        }

        [HttpGet("Language/{id}")]
        public Task<Response<LanguageDTO>> GetLanguage([FromRoute] Guid languageId, [FromHeader(Name = "RoleId")] Guid? roleId)
        {
            return this.app.Services.Language.GetLanguage(languageId, roleId);
        }

        [HttpGet("Languages")]
        public Task<Response<List<LanguageDTO>>> GetLanguages([FromHeader(Name = "RoleId")] Guid? roleId)
        {
            return this.app.Services.Language.GetLanguages(roleId);
        }

        [HttpDelete("Language/{id}")]
        public Task<Response> DeleteLanguage([FromRoute] Guid languageId, [FromHeader(Name = "RoleId")] Guid? roleId)
        {
            return this.app.Services.Language.DeleteLanguage(languageId, roleId);
        }
    }
}
