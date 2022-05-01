using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IRepositoryManager
    {
        IQuestionRepo Question { get; }

        IUserRepo User { get; }
    }
}
