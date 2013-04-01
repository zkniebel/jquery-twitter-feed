using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TweetNET.Requests.Parameters {
    public class RequestParameterCollection : List<RequestParameter> {
        public void Add(string key, string value) {
            RemoveAll(i => i.Key == key);
            Add(new RequestParameter(key, value));
        }
    }
}
