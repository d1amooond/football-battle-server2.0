using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface ILanguageRepo : IBaseRepo
    {
        Task<List<Language>> GetLanguages();
    }
}
