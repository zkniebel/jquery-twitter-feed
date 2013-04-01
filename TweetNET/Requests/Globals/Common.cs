using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TweetNET.Requests.Globals {
    public class Common {
        public const string REQUEST_METHOD_GET = "GET";
        public const string REQUEST_METHOD_POST = "POST";

        public const string RESOURCE_URL_USER_TIMELINE = "https://api.twitter.com/1.1/statuses/user_timeline.json";
        public const string RESOURCE_URL_STATUS_BY_ID = "https://api.twitter.com/1.1/statuses/show.json";

        public const string STRING_FORMAT_AMPERSAND_DELIM = "{0}&{1}";
        public const string STRING_FORMAT_QUESTION_MARK_DELIM = "{0}?{1}";

        public const string COMMON_STRING_AMPERSAND = "&";
    }
}
