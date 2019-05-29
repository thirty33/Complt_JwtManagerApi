using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Complet_JwtManager_Api.Domain.Services
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}
