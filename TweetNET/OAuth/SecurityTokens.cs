using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TweetNET.OAuth {
    /// <summary>
    /// Class to hold all of the authorization tokens and keys assigned by Twitter
    /// </summary>
    public class SecurityTokens {
        private string _ConsumerKey;
        private string _ConsumerSecret;
        private string _OAuthToken;
        private string _OAuthTokenSecret;

        /// <summary>
        /// Consumer Key assigned by Twitter
        /// </summary>
        public string ConsumerKey { 
            get {
                return _ConsumerKey;
            }
            set {
                _ConsumerKey = value;
            }
        }
        /// <summary>
        /// Consumer Secret assigned by Twitter
        /// </summary>
        public string ConsumerSecret {
            get {
                return _ConsumerSecret;
            }
            set {
                _ConsumerSecret = value;
            }
        }
        /// <summary>
        /// oAuth Token assigned by Twitter
        /// </summary>
        public string OAuthToken {
            get {
                return _OAuthToken;
            }
            set {
                _OAuthToken = value;
            }
        }
        /// <summary>
        /// oAuth Token Secret assigned by Twitter
        /// </summary>
        public string OAuthTokenSecret {
            get {
                return _OAuthTokenSecret;
            }
            set {
                _OAuthTokenSecret = value;
            }
        }

        /// <summary>
        /// Creates a new SecurityTokens instance
        /// </summary>
        /// <param name="ConsumerKey">Consumer Key assigned by Twitter</param>
        /// <param name="ConsumerSecret">Consumer Secret assigned by Twitter</param>
        /// <param name="OAuthToken">oAuth Token assigned by Twitter</param>
        /// <param name="OAuthTokenSecret">oAuth Token Secret assigned by Twitter</param>
        public SecurityTokens(string ConsumerKey, string ConsumerSecret, string OAuthToken, string OAuthTokenSecret) {
            _ConsumerKey = ConsumerKey;
            _ConsumerSecret = ConsumerSecret;
            _OAuthToken = OAuthToken;
            _OAuthTokenSecret = OAuthTokenSecret;
        }
    }
}
