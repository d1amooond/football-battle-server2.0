using Application.General;
using Application.Services.Question;

namespace Application.Services
{
    public class ServicesManager : IServicesManager
    {
        public ServicesManager(Context app) => this.app = app;
        public QuestionService Question => question ??= new QuestionService(app);
        private QuestionService question;

        public UserService User => user ??= new UserService(app);
        private UserService user;

        public RoleService Role => role ??= new RoleService(app);
        private RoleService role;

        public LanguageService Language => language ??= new LanguageService(app);
        private LanguageService language;

        private Context app;
    }
}
