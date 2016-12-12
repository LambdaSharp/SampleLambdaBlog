using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace SampleApiGatewayAuthenticator {

    public class AuthenticationRequest {

        //--- Properties ---
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("authorizationToken")]
        public string AuthorizationToken { get; set; }

        [JsonProperty("methodArn")]
        public string MethodArn { get; set; }
    }
}