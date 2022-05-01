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

namespace Football.Battle.Server.Controllers.Question
{
    [Authorize]
    [ApiController]
    public class QuestionController : ControllerBase
    {

        private readonly Context app;
        private readonly IFootballDBContext context;

        public QuestionController(IFootballDBContext context, IConfiguration configuration)
        {
            this.app = new Context(context, configuration);
        }

        [HttpGet("Questions")]
        public Task<Response<List<QuestionDTO>>> GetQuestions()
        {
            return app.Services.Question.GetQuestions();
        }

        [HttpPost("Questions")]
        public Task<Response<QuestionDTO>> CreateQuestion(QuestionRequest question)
        {
            return app.Services.Question.CreateQuestion(question);
        }

        [HttpPost("Questions/Draft")]
        public Task<Response<QuestionDTO>> DraftQuestion(DraftQuestionRequest question)
        {
            return app.Services.Question.DraftQuestion(question);
        }
    }
}
