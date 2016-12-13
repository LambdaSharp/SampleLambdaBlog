using System;

using Amazon.Lambda.Core;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializerAttribute(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace SampleApiGatewayAuthorizer {

    public class Function {

        //--- Class Methods ---
        private static AuthenticationResponse GeneratePolicy(string principalId, string effect, string resource) {
            var authResponse = new AuthenticationResponse {
                PrincipalId = principalId,
                PolicyDocument = new PolicyDocument {
                    Version = "2012-10-17",
                    Statement = new[] {
                        new PolicyStatement {
                            Action = "execute-api:Invoke",
                            Effect = effect,
                            Resource = resource
                        }
                    }
                }
            };
            return authResponse;
        }

        //--- Methods ---
        public AuthenticationResponse Authenticate(AuthenticationRequest authenticationRequest) {
            Console.WriteLine(JsonConvert.SerializeObject(authenticationRequest));
            if(authenticationRequest.AuthorizationToken == "foo") {
                return GeneratePolicy("*", "Allow", authenticationRequest.MethodArn);
            } else {
                return GeneratePolicy("*", "Deny", authenticationRequest.MethodArn);
            }
        }
    }
}
