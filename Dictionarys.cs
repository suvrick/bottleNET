
using System.Collections.Generic;

namespace bottlelib
{
    public static class Dictionarys
    {
        public static Dictionary<NetType, string[]> INPUT_SOCIAL_WORDS = new Dictionary<NetType, string[]>()
        {
            {
                NetType.OK, new []{ "ok.html", "api=ok" }
            },
            {
                NetType.VK, new []{ "vk.html", "type=vk" }
            },
            {
                NetType.MM, new []{ "mm.html", "type=mm" }
            },
            {
                NetType.FS, new []{ "fs.html" }
            },
            {
                NetType.IM, new []{ "sa.html", "type=sa" }
            }
        };

        public static Dictionary<NetType, string[]> INPUT_QUERY_WORDS = new Dictionary<NetType, string[]>()
        {
            {
                NetType.OK, new []{ "logged_user_id", "auth_sig", "session_key" }
            },
            {
                NetType.VK, new []{ "viewer_id", "auth_key", "access_token" }
            },
            {
                NetType.MM, new []{ "vid", "authentication_key", "" }
            },
            {
                NetType.FS, new []{ "userId", "authKey", "" }
            },
            {
                NetType.IM, new []{ "userId", "authKey", "" }
            },
            {
                NetType.NN, new []{ "", "", "" }
            }
        };
 }
}


