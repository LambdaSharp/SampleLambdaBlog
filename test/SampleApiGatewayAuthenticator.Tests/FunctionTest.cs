
using Xunit;
using Amazon.Lambda.TestUtilities;

namespace SampleApiGatewayAuthenticator.Tests {
    public class FunctionTest {

        //--- Methods ---
        [Fact]
        public void TestToUpperFunction() {

            // Invoke the lambda function and confirm the string was upper cased.
            var function = new Function();
            var context = new TestLambdaContext();
            var request = new AuthenticationRequest();
            var response = function.Authenticate(request);
            Assert.Equal("Deny", response.PolicyDocument.Statement[0].Effect);
        }
    }
}
