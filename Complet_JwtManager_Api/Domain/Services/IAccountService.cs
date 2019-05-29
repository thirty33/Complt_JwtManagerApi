using Complet_JwtManager_Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Complet_JwtManager_Api.Domain.Services
{
    public interface IAccountService
    {
        Task SignUp(string username, string password);
        JsonWebToken SignIn(string username, string password);
        JsonWebToken RefreshAccessToken(string token);
        void RevokeRefreshToken(string token);
    }
}
