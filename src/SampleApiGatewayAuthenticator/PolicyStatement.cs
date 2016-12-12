using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace SampleApiGatewayAuthenticator {

    public class PolicyStatement {

        //--- Properties ---
        public string Action { get; set; }
        public string Effect { get; set; }
        public string Resource { get; set; }
    }
}
