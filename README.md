# Sample Lambda# Blog
Sample serverless blog engine powered by AWS C# Lambda functions.

This repository shows how to build a sample blog engine using AWS C# lambda functions. The repository uses multiple
branches that map from simple to advanced implementations.

## Getting Started
To begin, make sure the following tools are installed.

1. Install .NET Core 1.0
2. Install nodejs
3. Install Yeoman `npm install -g yo generator-aws-lambda-dotnet`
4. Create a IAM user in the AWS console (e.g. `lambdasharp`)
5. Create a `lambdasharp` AWS configuration in `~/.aws/credentials`

Now you should be ready to publish!

6. Go in `src/SampleLambdaBlog` and run `dotnet restore`
7. Go in `src/SampleApiGatewayAuthenticator` and run `dotnet restore`

## Configuration
By default, this sample blog will  create the DynamoDB table as part of the CloudFormation template. You can edit
`src/SampleLambdaBlog/aws-lambda-tools-defaults.json` to adjust defaults for publishing. Please review `profile` and
`region`

**BEWARE** by default, the API Gateway created by the CloudFormation stack is public and vulnerable to abuse!

1. Create `SampleLambdaBlog-Authenticate` role
2. Add `AWSLambdaFullAccess` as policy
3. Add `AWSLambdaBasicExecutionRole` as policy
4. Click `Create`
5. In API Gateway, add `SampleLambdaBlogAuthenticate` as authorizer

6. Select Cognito
7. Create a user pool named `LambdaSharpBlogUsers`
8. Click `Review defaults`

## Using the SampleLambdaBlog API

You can find the URI for your SampleLambdaBlog API as follows:
1. Open the AWS console
2. Select `API Gateway`
3. Select `SampleLambdaBlog`
4. Select `Dashboard`

The URI will be shown at the top of the page. The URIs in the examples below will NOT WORK for you unless you replace the URI!

### Get all published blog posts
The following `curl` command will return an empty list `[]` unless a blog entry was added.
```
curl https://SampleLambdaBlog.execute-api.us-east-1.amazonaws.com/Prod/
```

### Publish a blog post
The following `curl` command will return the GUID of the new blog entry when it succeeds.
```
curl -X PUT -H "Content-Type: application/json" -d '{"Name":"John","Content":"What John wrote."}' https://SampleLambdaBlog.execute-api.us-east-1.amazonaws.com/Prod/
```

