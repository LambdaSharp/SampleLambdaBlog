using Newtonsoft.Json;

namespace SampleApiGatewayAuthorizer {

    public class AuthenticationResponse {

        //--- Properties ---
        [JsonPropertyAttribute("principalId")]
        public string PrincipalId { get; set; }

        [JsonProperty("policyDocument")]
        public PolicyDocument PolicyDocument { get; set; }
    }
}
