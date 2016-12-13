# Sample Lambda# Blog
Sample serverless blog engine powered by AWS C# Lambda functions.

This repository shows how to build a sample blog engine using AWS C# lambda functions. The repository uses multiple
branches that map from simple to advanced implementations.

## Step 1: Install Prequesite Tools
To begin, make sure the following tools are installed.

1. Install [.NET Core 1.0](https://www.microsoft.com/net/core#windowsvs2015)
2. Install [Nodejs](https://nodejs.org/en/)
3. Install [Yeoman](http://yeoman.io/codelab/setup.html)
4. Install AWS C# Lambda generator: `npm install -g yo generator-aws-lambda-dotnet`

## Step 2: Setup AWS Account
1. Create a IAM user in the AWS console (e.g. `lambdasharp`)
2. Create a `lambdasharp` AWS configuration in `~/.aws/credentials`
3. Create an S3 bucket (e.g. `lambdasharp-bucket`)
4. (optional) *Add a lifecycle policy to auto-delete files after 90 days*

## Step 3: Publish API Gateway Authorizer

### Create a role for the API Gateway authorizer
1. Go to IAM service in AWS console
2. Create a new role called `SampleLambdaBlog-Authorizer`
3. Attach `AWSLambdaBasicExecutionRole` and `AWSLambdaFullAccess` as policies

### Publish API Gateway Authorizer
1. Go into `src/SampleApiGatewayAuthorizer` 
2. Run `dotnet restore`
3. Run `dotnet lambda deploy-function` 

You should now have a published authorizer function!

## Step 4: Publish API Gateway

### Publish API Gateway 
1. Go into `src/SampleLambdaBlog` 
2. Run `dotnet restore`
3. Run `dotnet lambda deploy-serverless -sn SampleLambdaBlog -sb lambdasharp-bucket` 

**NOTE:** by default, this sample blog will create the DynamoDB table as part of the CloudFormation template. You can edit
`src/SampleLambdaBlog/aws-lambda-tools-defaults.json` to adjust defaults for publishing.

**BEWARE** by default, the API Gateway created by the `dotnet lambda deploy-serverless` is public and vulnerable to abuse.
Make sure to restrict access by configuring the API Gateway authorizer!

### Configure the API Gateway Authorizer
1. Go to API Gateway in AWS console
2. Select `SampleLambdaBlog`
3. Select `Authorizers`
4. Select `us-east-1` as Lambda region
5. Enter `SampleApiGatewayAuthorizer` as Lambda function name and Authorizer name
6. Click `Create`

### Use API Gatway Authorizer for `PUT` and `DELETE` methods
1. Click on `Resources`
2. Click on `DELETE`
3. Click on `Method Request`
4. Click on the pen next to `Authorization` and select `SampleApiGatewayAuthorizer`
5. Click checkmark to confirm selection
6. Repeat for `PUT` method

## Step 5: Using the SampleLambdaBlog API

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
curl -X PUT -H "Authorization: foo" -H "Content-Type: application/json" -d '{"Name":"John","Content":"What John wrote."}' https://SampleLambdaBlog.execute-api.us-east-1.amazonaws.com/Prod/
```
