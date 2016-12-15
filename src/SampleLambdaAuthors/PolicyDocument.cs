using System.Collections.Generic;

namespace SampleLambdaAuthors {

    public class PolicyDocument {

        //--- Properties ---
        public string Version { get; set; }
        public IList<PolicyStatement> Statement { get; set; }
    }
}
