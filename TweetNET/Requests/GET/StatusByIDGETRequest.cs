using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TweetNET.OAuth;
using TweetNET.Requests.Parameters;

namespace TweetNET.Requests.GET {
    public class StatusByIDGETRequest : Request {
        private string ID_Str = "id";
        private string Trim_User_Str = "trim_user";
        private string Include_My_Retweet_Str = "include_my_retweet";
        private string Include_Entities_Str = "include_entities";

        private string _ID = string.Empty;
        private string _Trim_User = string.Empty;
        private string _Include_My_Retweet = string.Empty;
        private string _Include_Entities = string.Empty;

        /// <summary>
        /// The numerical ID of the desired Tweet.
        /// </summary>
        /// <example>Example Values: 123</example>
        public string ID {
            get {
                return _ID;
            }
            set {
                RequestParams.Add(ID_Str, value);
            }
        }
        /// <summary>
        /// When set to either true, t or 1, each tweet returned in a timeline will include a user object including 
        /// only the status authors numerical ID. Omit this parameter to receive the complete user object.
        /// </summary>
        /// <example>Example Values: true</example>
        public string Trim_User {
            get {
                return _Trim_User;
            }
            set {
                RequestParams.Add(Trim_User_Str, value);
            }
        }
        /// <summary>
        /// When set to either true, t or 1, any Tweets returned that have been retweeted by the authenticating 
        /// user will include an additional current_user_retweet node, containing the ID of the source status for 
        /// the retweet.
        /// </summary>
        /// <example>Example Values: true</example>
        public string Include_My_Retweet {
            get {
                return _Include_My_Retweet;
            }
            set {
                RequestParams.Add(Include_My_Retweet_Str, value);
            }
        }
        /// <summary>
        /// The entities node will be disincluded when set to false.
        /// </summary>
        /// <example>Example Values: false</example>
        public string Include_Entities {
            get {
                return _Include_Entities;
            }
            set {
                RequestParams.Add(Include_Entities_Str, value);
            }
        }

        /// <summary>
        /// Creates a new StatusByIDGETRequest instance
        /// </summary>
        /// <param name="id">ID of the tweet to be retrieved</param>
        /// <param name="oAuthTokens">oAuth security keys, tokens and secrets assigned by Twitter</param>
        public StatusByIDGETRequest(string id, SecurityTokens oAuthTokens) 
            : base(Globals.Common.REQUEST_METHOD_GET, Globals.Common.RESOURCE_URL_STATUS_BY_ID, new RequestParameterCollection(), oAuthTokens) {
                Expect100Continue = false;
                ID = id;
        }
    }
}
