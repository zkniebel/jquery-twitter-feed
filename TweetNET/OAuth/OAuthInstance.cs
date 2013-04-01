using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using TweetNET.Requests.Parameters;

namespace TweetNET.OAuth {
    /// <summary>
    /// Class to hold methods for generating required oAuth data and structures for authenticating a request to the Twitter API
    /// </summary>
    public class OAuthInstance {
        /// <summary>
        /// oAuth Version (default: "1.0")
        /// </summary>
        private string OAuthVersion = "1.0";
        /// <summary>
        /// oAuth Signature Mehtod to be used (default: "HMAC-SHA1")
        /// </summary>
        private string OAuthSignatureMethod = "HMAC-SHA1";

        private SecurityTokens _OAuthTokens;
        private string _OAuthNonce;
        private string _OAuthTimeStamp;

        /// <summary>
        /// Security object containing the necessary Twitter-app-specific keys and tokens necessary for oAuth authentication
        /// </summary>
        public SecurityTokens Tokens {
            get {
                return _OAuthTokens;
            }
        }
        /// <summary>
        /// Unique, ASCII-encoded, base 64 timestamp for request authorization
        /// </summary>
        public string OAuthNonce {
            get {
                return _OAuthNonce;
            }
        }
        /// <summary>
        /// Unique, base 64 timestamp for request authorization
        /// </summary>
        public string OAuthTimeStamp {
            get {
                return _OAuthTimeStamp;
            }
        }

        /// <summary>
        /// Creates a new OAuthInstance instance
        /// </summary>
        /// <param name="tokens">SecurityTokens object containing all necessary keys and tokens for oAuth authentication</param>
        public OAuthInstance(SecurityTokens tokens) {
            _OAuthTokens = tokens;
            _OAuthNonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            _OAuthTimeStamp = Convert.ToInt64(
                (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds
            ).ToString();
        }

        /// <summary>
        /// Gets the OAuth security part of the base string for a web request
        /// </summary>
        /// <param name="requestParams">Collection of request-specific parameters</param>
        /// <returns>String - OAuth portion of base string</returns>
        public string GetOAuthBaseString(RequestParameterCollection requestParams) {
            var reqParams = new RequestParameterCollection();
            reqParams.AddRange(requestParams);
            reqParams.Add("oauth_consumer_key", Tokens.ConsumerKey);
            reqParams.Add("oauth_nonce", OAuthNonce);
            reqParams.Add("oauth_signature_method", OAuthSignatureMethod);
            reqParams.Add("oauth_timestamp", OAuthTimeStamp);
            reqParams.Add("oauth_token", Tokens.OAuthToken);
            reqParams.Add("oauth_version", OAuthVersion);

            reqParams.Sort();
            
            var baseString = string.Join(
                TweetNET.Requests.Globals.Common.COMMON_STRING_AMPERSAND,
                reqParams);
            
            return baseString;
        }

        /// <summary>
        /// Gets the oAuth authorization header, based on the given oAuth Signature string
        /// </summary>
        /// <param name="signature">oAuth Signature string</param>
        /// <returns>unique oAuth authorization header for this OAuthInstance</returns>
        public string GetOAuthHeader(string signature) {
            //create the request header
            var headerFormat = "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
                               "oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
                               "oauth_token=\"{4}\", oauth_signature=\"{5}\", " +
                               "oauth_version=\"{6}\"";

            return string.Format(headerFormat,
                            Uri.EscapeDataString(OAuthNonce),
                            Uri.EscapeDataString(OAuthSignatureMethod),
                            Uri.EscapeDataString(OAuthTimeStamp),
                            Uri.EscapeDataString(Tokens.ConsumerKey),
                            Uri.EscapeDataString(Tokens.OAuthToken),
                            Uri.EscapeDataString(signature),
                            Uri.EscapeDataString(OAuthVersion)
                    );
        }

        /// <summary>
        /// Gets the parsed composite key for the authorization secrets stored the this instance's Token property
        /// </summary>
        /// <returns>Parsed composite key string</returns>
        public string GetCompositeKey() {
            return GetCompositeKey(Tokens.ConsumerSecret, Tokens.OAuthTokenSecret);
        }

        /// <summary>
        /// Gets the parsed composite key for the given authorization secrets
        /// </summary>
        /// <param name="consumerSecret">Consumer Secret assigned by Twitter</param>
        /// <param name="oAuthTokenSecret">oAuth Token Secret assigned by Twitter</param>
        /// <returns>Parsed composite key string</returns>
        public static string GetCompositeKey(string consumerSecret, string oAuthTokenSecret) {
            return string.Concat(
                    Uri.EscapeDataString(consumerSecret),
                    TweetNET.Requests.Globals.Common.COMMON_STRING_AMPERSAND, 
                    Uri.EscapeDataString(oAuthTokenSecret)
                );
        }

        /// <summary>
        /// Gets the oAuth signature based on the given base string
        /// </summary>
        /// <param name="baseString">The basestring generated for the request that this signature is to be used for</param>
        /// <returns>oAuth signature string</returns>
        public string GetOAuthSignature(string baseString) {
            return GetOAuthSignature(baseString, GetCompositeKey());
        }

        /// <summary>
        /// Gets the oAuth signature based on the given base string and composite key
        /// </summary>
        /// <param name="baseString">The basestring generated for the request that this signature is to be used for</param>
        /// <param name="compositeKey">The composite key parsed from the authorization secrets assigned by Twitter</param>
        /// <returns>oAuth signature string</returns>
        public string GetOAuthSignature(string baseString, string compositeKey) {
            string signature;
            using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey))) {
                signature = Convert.ToBase64String(
                    hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
            }

            return signature;
        }
    }
}
