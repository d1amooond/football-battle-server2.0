using Application.Dtos;
using Utils.Extensions;
namespace Application.Extensions
{
    public static class QuestionExtension
    {
        public static QuestionDTO ToDTO(this Domain.Entities.Question question)
        {
            return new QuestionDTO
            {
                Id = question.Id.AsGuid(),
                CorrectAnswer = question.CorrectAnswer,
                Answers = question.Answers,
                CorrectAnswerId = question.CorrectAnswerId.AsGuid(),
                Description = question.Description,
                EndDate = question.EndDate,
                Image = question.Image,
                Notes = question.Notes,
                Status = question.Status,
                Category = question.Category,
                OwnerId = question.OwnerId.AsGuid()
            };
        }
    }
}
