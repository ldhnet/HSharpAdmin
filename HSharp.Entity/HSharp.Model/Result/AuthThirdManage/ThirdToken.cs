using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSharp.Model.Result.AuthThirdManage
{
    public class AuthThirdToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
        public string example_parameter { get; set; }
    }

    public class ThirdAuthorizeResult
    {
        public string code { get; set; }
        public string error { get; set; }
        public string error_description { get; set; }
        public int state { get; set; }
    }
}
