using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TweetNET.Requests.Parameters {
    /// <summary>
    /// Class representation of a request-specific parameter (e.g., screen_name, count, etc.)
    /// </summary>
    public class RequestParameter : IComparable<RequestParameter> {
        private string _Key;
        private string _Value; 

        /// <summary>
        /// The parameter key accepted by the Twitter API
        /// </summary>
        public string Key {
            get {
                return _Key;
            }
        }
        /// <summary>
        /// The value to be assigned to the parameter
        /// </summary>
        public string Value {
            get {
                return _Value;
            }
        }

        /// <summary>
        /// Creates a new RequestParameter instance
        /// </summary>
        /// <param name="key">Parameter key</param>
        /// <param name="value">Value of parameter</param>
        public RequestParameter(string key, string value) {
            _Key = key;
            _Value = value;
        }

        /// <summary>
        /// Gets the string representation of this RequestParameter object
        /// </summary>
        /// <returns>Parsed key-value string</returns>
        public override string ToString() {
            return string.Format("{0}={1}", Uri.EscapeDataString(Key), Uri.EscapeDataString(Value));
        }
        
        /// <summary>
        /// Compares two RequestParameters
        /// </summary>
        /// <param name="other">The RequestParameter to be compared with</param>
        /// <returns>int representing the result of the comparison (-1 for less, 0 for equal, 1 for greater)</returns>
        public int CompareTo(RequestParameter other) {
            if (Key == other.Key) {
                return string.Compare(Value, other.Value);
            } else {
                return string.Compare(Key, other.Key);
            }
        }
    }
}
