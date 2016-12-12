using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace SampleApiGatewayAuthenticator {

    public class AuthenticationResponse {

        //--- Properties ---
        [JsonPropertyAttribute("principalId")]
        public string PrincipalId { get; set; }

        [JsonProperty("policyDocument")]
        public PolicyDocument PolicyDocument { get; set; }
    }
}
