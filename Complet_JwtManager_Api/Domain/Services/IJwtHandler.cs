using Complet_JwtManager_Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Complet_JwtManager_Api.Domain.Services
{
    public interface IJwtHandler
    {
        JsonWebToken Create(string username);
    }
}
