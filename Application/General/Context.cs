using DataAccess.DBContext;
using DataAccess.Repository;
using Microsoft.Extensions.Configuration;

namespace Application.General
{
    public class Context
    {
        public Services.IServicesManager Services { get; private set; }
        public IRepositoryManager Repository { get; private set; }
        public IConfiguration Configuration { get; private set; } 

        public Context(IFootballDBContext context, IConfiguration configuration)
        {
            this.Services = new Application.Services.ServicesManager(this);
            this.Repository = new DataAccess.Repository.RepositoryManager(context);
            this.Configuration = configuration;
        }
    }
}
