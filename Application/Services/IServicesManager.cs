using Application.Services.Question;
namespace Application.Services
{
    public interface IServicesManager
    {
        QuestionService Question { get; }
        UserService User { get; }

        RoleService Role { get; }

        LanguageService Language { get; }

        CategoryService Category { get; }
    }
}
