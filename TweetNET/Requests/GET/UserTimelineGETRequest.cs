using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TweetNET.OAuth;
using TweetNET.Requests.Parameters;
using System.Net;

namespace TweetNET.Requests.GET {
    public class UserTimelineGETRequest : Request {
        private string User_ID_Str = "user_id";
        private string Screen_Name_Str = "screen_name";
        private string Since_ID_Str = "since_id";
        private string Count_Str = "count";
        private string Max_ID_Str = "max_id";
        private string Trim_User_Str = "trim_user";
        private string Exclude_Replies_Str = "exclude_replies";
        private string Contributor_Details_Str = "contributor_details";
        private string Include_RTS_Str = "include_rts";

        private string _User_ID = string.Empty;
        private string _Screen_Name = string.Empty;
        private string _Since_ID = string.Empty;
        private string _Count = string.Empty;
        private string _Max_ID = string.Empty;
        private string _Trim_User = string.Empty;
        private string _Exclude_Replies = string.Empty;
        private string _Contributor_Details = string.Empty;
        private string _Include_RTS = string.Empty;

        /// <summary>
        /// (one of User_ID and Screen_Name must be set) - 
        /// ID of the user for whom to return results for.
        /// </summary>
        /// <example>Example Values: "12345"</example>
        public string User_ID {
            get {
                return _User_ID;
            }
            set {
                RequestParams.Add(User_ID_Str, value);
                _User_ID = value;
            }
        }
        /// <summary>
        /// (one of User_ID and Screen_Name must be set) - 
        /// The screen name of the user for whom to return results for.
        /// </summary>
        /// <example>Example Values: "noradio"</example>
        public string Screen_Name {
            get {
                return _Screen_Name;
            }
            set {
                RequestParams.Add(Screen_Name_Str, value);
                _Screen_Name = value;
            }
        }
        /// <summary>
        /// (optional) - 
        /// Returns results with an ID greater than (that is, more recent than) the specified ID. 
        /// There are limits to the number of Tweets which can be accessed through the API. If the 
        /// limit of Tweets has occured since the since_id, the since_id will be forced to the 
        /// oldest ID available.
        /// </summary>
        /// <example>Example Values: "12345"</example>
        public string Since_ID {
            get {
                return _Since_ID;
            }
            set {
                RequestParams.Add(Since_ID_Str, value);
                _Since_ID = value;
            }
        }
        /// <summary>
        /// (optional) - 
        /// Specifies the number of tweets to try and retrieve, up to a maximum of 200 per distinct
        /// request. The value of count is best thought of as a limit to the number of tweets to 
        /// return because suspended or deleted content is removed after the count has been applied. 
        /// We include retweets in the count, even if include_rts is not supplied. It is recommended 
        /// you always send include_rts=1 when using this API method.
        /// </summary>
        /// <example>Example Values: "25"</example>
        public string Count {
            get {
                return _Count;
            }
            set {
                RequestParams.Add(Count_Str, value);
                _Count = value;
            }
        }
        /// <summary>
        /// (optional) - 
        /// Returns results with an ID less than (that is, older than) or equal to the specified ID.
        /// </summary>
        /// <example>Example Values: "54321"</example>
        public string Max_ID {
            get {
                return _Max_ID;
            }
            set {
                RequestParams.Add(Max_ID_Str, value);
                _Max_ID = value;
            }
        }
        /// <summary>
        /// (optional) - 
        /// When set to either true, t or 1, each tweet returned in a timeline will include a user 
        /// object including only the status authors numerical ID. Omit this parameter to receive the
        /// complete user object.
        /// </summary>
        /// <example>Example Values: "true"</example>
        public string Trim_User {
            get {
                return _Trim_User;
            }
            set {
                RequestParams.Add(Trim_User_Str, value);
                _Trim_User = value;
            }
        }
        /// <summary>
        /// (optional) - 
        /// This parameter will prevent replies from appearing in the returned timeline. Using 
        /// exclude_replies with the count parameter will mean you will receive up-to count tweets — 
        /// this is because the count parameter retrieves that many tweets before filtering out 
        /// retweets and replies. This parameter is only supported for JSON and XML responses.
        /// </summary>
        /// <example>Example Values: "true"</example>
        public string Exclude_Replies {
            get {
                return _Exclude_Replies;
            }
            set {
                RequestParams.Add(Exclude_Replies_Str, value);
                _Exclude_Replies = value;
            }
        }
        /// <summary>
        /// (optional) - 
        /// This parameter enhances the contributors element of the status response to include the 
        /// screen_name of the contributor. By default only the user_id of the contributor is 
        /// included.
        /// </summary>
        /// <example>Example Values: "true"</example>
        public string Contributor_Details {
            get {
                return _Contributor_Details;
            }
            set {
                RequestParams.Add(Contributor_Details_Str, value);
                _Contributor_Details = value;
            }
        }
        /// <summary>
        /// (optional) - 
        /// When set to false, the timeline will strip any native retweets (though they will still 
        /// count toward both the maximal length of the timeline and the slice selected by the 
        /// count parameter). Note: If you're using the trim_user parameter in conjunction with 
        /// include_rts, the retweets will still contain a full user object.
        /// </summary>
        /// <example>Example Values: "false"</example>
        public string Include_RTS {
            get {
                return _Include_RTS;
            }
            set {
                RequestParams.Add(Include_RTS_Str, value);
                _Include_RTS = value;
            }
        }

        /// <summary>
        /// Creates a new UserTimelineGETRequest instance
        /// </summary>
        /// <param name="oAuthTokens">oAuth keys, tokens and secrets used to authorize the request</param>
        public UserTimelineGETRequest(SecurityTokens oAuthTokens)
            : base(Globals.Common.REQUEST_METHOD_GET, Globals.Common.RESOURCE_URL_USER_TIMELINE, new RequestParameterCollection(), oAuthTokens) {
                Expect100Continue = false;
        }

        public override HttpWebRequest BuildRequest(string compositeKey) {
            if (Screen_Name != string.Empty || User_ID != string.Empty) {
                return base.BuildRequest(compositeKey);
            } else {
                throw new Exception("Request not built: A User ID or Screen Name must be supplied for this request");
            }
        }
    }
}
