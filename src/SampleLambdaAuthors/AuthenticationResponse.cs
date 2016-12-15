using Newtonsoft.Json;

namespace SampleLambdaAuthors {

    public class AuthenticationResponse {

        //--- Properties ---
        [JsonPropertyAttribute("principalId")]
        public string PrincipalId { get; set; }

        [JsonProperty("policyDocument")]
        public PolicyDocument PolicyDocument { get; set; }
    }
}
