using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;

using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializerAttribute(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace SampleLambdaAuthors {

    public class Functions {

        //--- Constants ---
        public const string ID_QUERY_STRING_NAME = "Id";
        private const string TABLENAME_ENVIRONMENT_VARIABLE_LOOKUP = "AuthorTable";

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

        //--- Fields ---
        private IDynamoDBContext DDBContext { get; set; }

        //--- Constructors ---
        /// <summary>
        /// Default constructor that Lambda will invoke.
        /// </summary>
        public Functions() {

            // Check to see if a table name was passed in through environment variables and if so
            // add the table mapping.
            var tableName = System.Environment.GetEnvironmentVariable(TABLENAME_ENVIRONMENT_VARIABLE_LOOKUP);
            if(!string.IsNullOrEmpty(tableName)) {
                AWSConfigsDynamoDB.Context.TypeMappings[typeof(Author)] = new Amazon.Util.TypeMapping(typeof(Author), tableName);
            }
            var config = new DynamoDBContextConfig { Conversion = DynamoDBEntryConversion.V2 };
            this.DDBContext = new DynamoDBContext(new AmazonDynamoDBClient(), config);
        }

        /// <summary>
        /// Constructor used for testing passing in a preconfigured DynamoDB client.
        /// </summary>
        /// <param name="ddbClient"></param>
        /// <param name="tableName"></param>
        public Functions(IAmazonDynamoDB ddbClient, string tableName) {
            if (!string.IsNullOrEmpty(tableName)) {
                AWSConfigsDynamoDB.Context.TypeMappings[typeof(Author)] = new Amazon.Util.TypeMapping(typeof(Author), tableName);
            }
            var config = new DynamoDBContextConfig { Conversion = DynamoDBEntryConversion.V2 };
            this.DDBContext = new DynamoDBContext(ddbClient, config);
        }

        //--- Methods ---

        /// <summary>
        /// A Lambda function that updates an author.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<APIGatewayProxyResponse> UpdateAuthorAsync(APIGatewayProxyRequest request, ILambdaContext context) {
            var author = JsonConvert.DeserializeObject<Author>(request?.Body);
            author.Id = Guid.NewGuid().ToString();
            context.Logger.LogLine($"Saving author with id {author.Id}");
            await DDBContext.SaveAsync<Author>(author);
            var response = new APIGatewayProxyResponse {
                StatusCode = (int)HttpStatusCode.OK,
                Body = author.Id.ToString(),
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
            return response;
        }

        /// <summary>
        /// A Lambda function that returns back a page worth of authors.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The list of blogs</returns>
        public async Task<APIGatewayProxyResponse> GetAuthorsAsync(APIGatewayProxyRequest request, ILambdaContext context) {
            context.Logger.LogLine("Getting authors");
            var search = this.DDBContext.ScanAsync<Author>(null);
            var page = await search.GetNextSetAsync();
            context.Logger.LogLine($"Found {page.Count} authors");
            var response = new APIGatewayProxyResponse {
                StatusCode = (int)HttpStatusCode.OK,
                Body = JsonConvert.SerializeObject(page),
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
            return response;
        }

        /// <summary>
        /// A Lambda function that removes an author from the DynamoDB table.
        /// </summary>
        /// <param name="request"></param>
        public async Task<APIGatewayProxyResponse> RemoveAuthorAsync(APIGatewayProxyRequest request, ILambdaContext context) {
            var authorId = request?.PathParameters[ID_QUERY_STRING_NAME];
            if(string.IsNullOrEmpty(authorId)) {
                authorId = request?.QueryStringParameters[ID_QUERY_STRING_NAME];
            }
            if(string.IsNullOrEmpty(authorId)) {
                return new APIGatewayProxyResponse {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Body = "Missing required parameter authorId"
                };
            }
            context.Logger.LogLine($"Deleting author with id {authorId}");
            await this.DDBContext.DeleteAsync<Author>(authorId);
            return new APIGatewayProxyResponse {
                StatusCode = (int)HttpStatusCode.OK
            };
        }

        public Task<AuthenticationResponse> AuthorizeAsync(AuthenticationRequest authenticationRequest) {
            Console.WriteLine(JsonConvert.SerializeObject(authenticationRequest));
            if(authenticationRequest.AuthorizationToken == "foo") {
                return Task.FromResult(GeneratePolicy("*", "Allow", authenticationRequest.MethodArn));
            } else {
                return Task.FromResult(GeneratePolicy("*", "Deny", authenticationRequest.MethodArn));
            }
        }
    }
}
