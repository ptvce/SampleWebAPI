using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebAPI
{
    public class AuthenticateViewModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
