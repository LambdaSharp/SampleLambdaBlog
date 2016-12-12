using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace SampleApiGatewayAuthenticator {

    public class PolicyDocument {

        //--- Properties ---
        public string Version { get; set; }
        public IList<PolicyStatement> Statement { get; set; }
    }
}
