using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Complet_JwtManager_Api.Helpers
{
    public class RefreshToken
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public bool Revoked { get; set; }
    }
}
