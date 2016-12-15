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
5. Install [AWS CLI](https://aws.amazon.com/cli/)



## Step 2: Setup AWS Account
1. Create a IAM user in the AWS console (e.g. `lambdasharp`)
2. Create a `lambdasharp` AWS configuration in `~/.aws/credentials`
3. Create an S3 bucket (e.g. `lambdasharp`)
4. (optional) *Add a lifecycle policy to auto-delete files after 90 days*

## Step 3: Setting up the Authors API

### Publish Authors API
1. Go into `src/SampleLambdaAuthors`
2. Run `dotnet restore`
3. Run `dotnet lambda deploy-serverless`

### Create a Usage Plan with an API Key
This usage plan is to protect yourself from abuse of the Authors API.

1. Go to API Gateway in AWS console
2. Click on Usage Plans
3. Click `Create`
4. Give it a name: `Basic Usage Plan`
5. Limit the rate to `5` requests per second
6. Limit the burst rate to `10` requests per second
7. Enable a quota of `10000` requests per `month`
8. Click `Next`
9. Click `Add` to a new row with API set to `SampleLambdaAuthorsApi` and stage set to `Prod`
10. Click the checkmark to the right of the row
11. Repeat these steps to `SampleLambdaAuthorsApi` and `Stage` (**NOTE:** you may need to way a bit to add the other row; just try again if you get an error message)
12. Click `Next`
13. Click `Create API Key and add to Usage Plan`
14. Set API Key name to `Basic API Key`
15. Click `Save`

### Configure acess to the Authors API
1. Click on `SampleLambdaAuthorsApi` in the left navigation pane
2. Click the `DELETE` method
3. Click on `Method Request`
4. Click on the pen next to `API Key Required` and select `true` then click the checkmark to confirm the selection
5. Repeate these steps for the `GET` and `PUT` methods
7. Click on the `Actions` button near the top
8. Select `Deploy API`
9. Select `Prod` under `Deployment stage`
10. Click `Deploy`

### Test Authors API
1. Go to `API Keys`
2. Select `Basic API Key`
3. Click `Show` next to `API Key`
4. Copy the API Key value and replace `API-KEY` in the test command below
5. Select `SampleLambdaAuthorsApi` in the left navigation pane
6. Select `Dashboard`
7. Copy the API URI at the top of the page and replace `AUTHORS-URI` in the text command below

The following `curl` command will return the GUID of the new blog entry when it succeeds.
```
curl -X PUT -H "x-api-key: API-KEY" -H "Content-Type: application/json" -d '{"Name":"John"}' AUTHORS-URI
```


## Step 4: Setting up the the Blog API

### Publish Blog API
1. Go into `src/SampleLambdaBlog`
2. Run `dotnet restore`
3. Run `dotnet lambda deploy-serverless`

### Configure the Blog PI Gateway Authorizer
1. Go to API Gateway in AWS console
2. Select `SampleLambdaBlogApi`
3. Select `Authorizers`
4. Select `us-east-1` as Lambda region
5. Select `SampleLambdaAuthorsApi-Authorize-*` as Lambda function
6. Enter `Authorizer` as name
7. Click `Create`

### Configure acess to the Blog API
1. Click on `Resources`
2. Click on `DELETE`
3. Click on `Method Request`
4. Click on the pen next to `Authorization` and select `SampleAuthorsAuthorizer`
5. Click checkmark to confirm selection
6. Repeat for `PUT` method
7. Click on the `Actions` button near the top
8. Select `Deploy API`
9. Select `Prod` under `Deployment stage`
10. Click `Deploy`















## Step 5: Using the SampleLambdaBlog API

You can find the URI for your `SampleLambdaBlog` API as follows:
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

# Notes

**BEWARE:** The default cloud-formation template used by the serverless deployment step creates the DynamoDB table. This table will be deleted with all its contents when you delete the cloud-formation stack!
`src/SampleLambdaBlog/aws-lambda-tools-defaults.json` to adjust defaults for publishing.

**BEWARE:** by default, the API Gateway created by the `dotnet lambda deploy-serverless` is public and vulnerable to abuse. Make sure to restrict access!
