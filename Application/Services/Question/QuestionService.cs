using Application.Dtos;
using Application.General;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Extensions;
using Application.Requests;
using Utils.Extensions;

namespace Application.Services.Question
{
    public class QuestionService
    {
        private readonly Context app;
        public QuestionService(Context app)
        {
            this.app = app;
        }
        async public Task<Response<List<QuestionDTO>>> GetQuestions()
        {
            var questions = await app.Repository.Question.GetQuestions();
            var questionsDTO = new List<QuestionDTO>();
            
            foreach (var question in questions)
            {
                questionsDTO.Add(question.ToDTO());
            }
            var response = new Response<List<QuestionDTO>>();

            return response.Success(questionsDTO);
        }

        async public Task<Response<QuestionDTO>> DraftQuestion(DraftQuestionRequest draftQuestion)
        {
            var response = new Response<QuestionDTO>();
            var languages = (int[])Enum.GetValues(typeof(Languages));

            if (string.IsNullOrWhiteSpace(draftQuestion.Description))
            {
                return response.Failure("Description of question can't be null or white space!");
            }

            if (draftQuestion.Answers.Count != 4)
            {
                return response.Failure("Number of answers should be 4!");
            }

            foreach (var answer in draftQuestion.Answers)
            {
                if (string.IsNullOrWhiteSpace(answer))
                {
                    return response.Failure("One or more of the answers equals null or white space!");
                }
            }

            if (string.IsNullOrWhiteSpace(draftQuestion.CorrectAnswer))
            {
                return response.Failure("Correct Answer cannot be null of white space!");
            }

            if (!draftQuestion.Answers.Contains(draftQuestion.CorrectAnswer))
            {
                return response.Failure("Answers doesn't contain this correct answer!");
            }

            var questionRequest = new QuestionRequest();

            questionRequest.Description = languages.Select(l => new Description()
            {
                Language = l,
                Content = draftQuestion.Description,
            }).ToList();

            questionRequest.Answers = draftQuestion.Answers.Select(a => new Answer
            {
                Variants = languages.Select(l => new AnswerVariant
                {
                    Language = l,
                    Content = a,
                }).ToList(),
            }).ToList();

            questionRequest.CorrectAnswer = new Answer
            {
                Variants = languages.Select(l => new AnswerVariant
                {
                    Language = l,
                    Content = draftQuestion.CorrectAnswer,
                }).ToList()
            };

            questionRequest.OwnerId = draftQuestion.OwnerId;
            questionRequest.Status = Statuses.Draft;
            questionRequest.Image = draftQuestion.Image;

            return await this.CreateQuestion(questionRequest);
        }

        async public Task<Response<QuestionDTO>> CreateQuestion(QuestionRequest questionRequest)
        {
            var response = new Response<QuestionDTO>();
            var languages = (int[])Enum.GetValues(typeof(Languages));

            bool descriptionContainsAllLanguages = languages.All(language =>
            questionRequest.Description.Any(d => d.Language == language));

            if (!descriptionContainsAllLanguages)
            {
                return response.Failure("Description should contain all languages!");
            }

            bool answersContainAllLanguages = languages.All(language =>
            questionRequest.Answers.SelectMany(a => a.Variants).Count(a => a.Language == language) == 4);

            if (!answersContainAllLanguages)
            {
                return response.Failure("Answers should contain 4 answers for all languages!");
            }

            if (questionRequest.CorrectAnswer == null)
            {
                return response.Failure("Correct answer is empty!");
            }

            if (questionRequest.Status != Statuses.Draft && questionRequest.Category == 0)
            {
                return response.Failure("Incorrect category!");
            }
            
            if (questionRequest.Status == 0)
            {
                questionRequest.Status = Statuses.Active;
            }

            foreach (var answer in questionRequest.Answers)
            {
                answer.Id = MongoDB.Bson.ObjectId.GenerateNewId();
            }

            var correctAnswer = questionRequest.Answers
                .FirstOrDefault(q => q.Variants.All(v => questionRequest.CorrectAnswer.Variants.Select(v => v.Content).Contains(v.Content)));

            var question = await this.app.Repository.Question.CreateQuestion(new Domain.Entities.Question
            {
                Description = questionRequest.Description,
                Answers = questionRequest.Answers,
                Notes = questionRequest.Notes,
                EndDate = questionRequest.EndDate,
                Image = questionRequest.Image,
                CorrectAnswer = correctAnswer,
                CorrectAnswerId = correctAnswer.Id.Value,
            });
            return response.Success(question.ToDTO());
        }
    }
}
