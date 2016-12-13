using Newtonsoft.Json;

namespace SampleApiGatewayAuthorizer {

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