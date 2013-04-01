using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using TweetNET.OAuth;
using TweetNET.Requests;
using TweetNET.Requests.Parameters;

namespace TweetNET {
    /// <summary>
    /// Abstract class to manage the building and sending of Twitter API requests
    /// </summary>
    public class Request {
        /// <summary>
        /// List of request-specific parameters
        /// </summary>
        public RequestParameterCollection RequestParams { get; set; }
        /// <summary>
        /// URL to which the request should be sent
        /// </summary>
        public string ResourceURL { get; set; }
        /// <summary>
        /// Method to be used for the request (GET or POST)
        /// </summary>
        public string RequestMethod { get; set; }
        /// <summary>
        /// OAuthInstance to generate all necessary oAuth headers, and signatures that are required for the request
        /// </summary>
        public OAuthInstance OAuth { get; set; }
        /// <summary>
        /// Determines whether or not the ServicePointManager should expect 100-Continue responses for POST requests (default: true)
        /// </summary>
        public bool Expect100Continue = true;
        /// <summary>
        /// Sets the content type of the request data to be sent (default: "application/x-www-form-urlencoded")
        /// </summary>
        public string ContentType = "application/x-www-form-urlencoded";

        /// <summary>
        /// Creates a new instance of the RequestManager class
        /// </summary>
        /// <param name="requestMethod">Method to be used for the request (GET or POST)</param>
        /// <param name="resourceURL">URL to which the request should be sent</param>
        /// <param name="requestParams">List of request-specific parameters</param>
        /// <param name="oAuthTokens">oAuth keys, tokens and secrets used to authorize the request</param>
        public Request(string requestMethod, string resourceURL, RequestParameterCollection requestParams, SecurityTokens oAuthTokens) {
            RequestMethod = requestMethod;
            ResourceURL = resourceURL;
            RequestParams = requestParams;
            OAuth = new OAuthInstance(oAuthTokens);
        }

        /// <summary>
        /// Gets the base string of the request
        /// </summary>
        /// <returns>Base string of the request</returns>
        public string GetBaseString() {
            var baseString = OAuth.GetOAuthBaseString(RequestParams);
            baseString = string.Concat(
                RequestMethod,
                TweetNET.Requests.Globals.Common.COMMON_STRING_AMPERSAND,
                Uri.EscapeDataString(ResourceURL), 
                TweetNET.Requests.Globals.Common.COMMON_STRING_AMPERSAND, 
                Uri.EscapeDataString(baseString));
            
            return baseString;
        }

        /// <summary>
        /// Builds an HttpWebRequest object for making a request to the Twitter API
        /// </summary>
        /// <returns>HttpWebRequest object</returns>
        public virtual HttpWebRequest BuildRequest() {
            return BuildRequest(OAuth.GetCompositeKey());
        }

        /// <summary>
        /// Builds an HttpWebRequest object for making a request to the Twitter API, using the given composite key
        /// </summary>
        /// <param name="compositeKey">Composite key to be used for the request</param>
        /// <returns>HttpWebRequest object</returns>
        public virtual HttpWebRequest BuildRequest(string compositeKey) {
            var baseString = GetBaseString();
            var signature = OAuth.GetOAuthSignature(baseString, compositeKey);
            var header = OAuth.GetOAuthHeader(signature);

            var parsedParams = string.Join(
                TweetNET.Requests.Globals.Common.COMMON_STRING_AMPERSAND,
                RequestParams);
            var requestURL = string.Format(
                TweetNET.Requests.Globals.Common.STRING_FORMAT_QUESTION_MARK_DELIM, 
                ResourceURL, 
                parsedParams);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestURL);
            request.Headers.Add("Authorization", header);
            request.Method = RequestMethod;
            request.ContentType = ContentType;

            return request;
        }

        /// <summary>
        /// Sends the given HttpWebRequest and returns the response
        /// </summary>
        /// <param name="request">object representation of the request to be sent</param>
        /// <returns>Response from the sent request</returns>
        public virtual WebResponse SendRequest(HttpWebRequest request) {
            ServicePointManager.Expect100Continue = Expect100Continue;
            return request.GetResponse();
        }
    }
}