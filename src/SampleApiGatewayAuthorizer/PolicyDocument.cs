namespace SampleApiGatewayAuthorizer {

    public class PolicyDocument {

        //--- Properties ---
        public string Version { get; set; }
        public IList<PolicyStatement> Statement { get; set; }
    }
}
