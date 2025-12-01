The main idea of this PoC is to implement the functionality to lend the books.

Follow the steps below to run the application.

-> Navigate to https://github.com/priyanandavaram/BookLendingServiceAWSSolution and clone the repository on your local machine.

-> Run the application and you'll displayed with swagger page.

-> Test the endpoints using swagger.


Build the solution using Visual Studio 2022 and follow the below steps to create an image in the Docker and ECR.

-> Open the terminal in Visual Studio or Command Prompt and navigate to the project folder where Dockerfile is located 

-> Run the below command to create a docker image (Make sure docker desktop is running)

   docker build -t booklendingserviceawssolution .

-> Run the below commands to create an ECR image within AWS (323432454298 is the Account ID of your AWS account)

   aws ecr get-login-password --region eu-north-1 | docker login --username AWS --password-stdin 323432454298.dkr.ecr.eu-north-1.amazonaws.com

   aws ecr create-repository --repository-name booklendingserviceawssolution

   docker tag booklendingserviceawssolution:latest 323432454298.dkr.ecr.eu-north-1.amazonaws.com/booklendingserviceawssolution:latest

   docker push 323432454298.dkr.ecr.eu-north-1.amazonaws.com/booklendingserviceawssolution:latest

-> Run the below commands to create an IAM role with Lambda execution permissions

   aws iam create-role --role-name booklendingservicelambdaRole --assume-role-policy-document file://C:\Users\autom\source\repos\BookLendingServiceAWSSolution\BookLendingServiceAWSSolution\trust-policy.json

   aws iam attach-role-policy --role-name booklendingservicelambdaRole --policy-arn arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole

-> Run the below command to create a Lambda function using the ECR image created in the previous steps.

   aws lambda create-function --function-name booklendingserviceawssolution-lambdafunction --package-type Image --code ImageUri=323432454298.dkr.ecr.eu-north-1.amazonaws.com/booklendingserviceawssolution:latest --role arn:aws:iam::323432454298:role/booklendingservicelambdaRole --timeout 30 --memory-size 1024

-> Once the Lambda function is created, navigate to AWS API Gateway console and create a new REST API.

   aws apigateway create-rest-api --name booklendingserviceawssolution-apigateway --endpoint-configuration types=REGIONAL

    Sample response:
    {
	    "id": "czpi202dx7",
	    "name": "booklendingserviceawssolution-apigateway",
	    "description": "",
	    "createdDate": "2024-06-15T10:00:00Z",
	    "version": "",
	    "warnings": [],
	    "binaryMediaTypes": [],
	    "minimumCompressionSize": -1,
	    "apiKeySource": "HEADER",
	    "endpointConfiguration": {
	 	   "types": [
	 		   "REGIONAL"
	 	   ]
	    },
	    "policy": null,
	     "tags": {}
	}

   aws apigateway get-resources --rest-api-id czpi202dx7 // czpi202dx7 - Replace the Id that we get in the above json response.

    Sample response:
    {
	     "items": [
	  	   {
	  		   "id": "itq8dn21d0",  		
	  		   "path": "/"
	  	   }
	     ]
    }

   aws apigateway create-resource --rest-api-id czpi202dx7 --parent-id itq8dn21d0 --path-part api // itq8dn21d0 - Replace the Id that we get in the above json response.

	Sample response:
	{
		   "items": [
			   {
				   "id": "gzhknt",
				   "parentId": "20e2gbb80k",
				   "pathPart": "api",
				   "path": "/api"
			   }
		   ]
    }

  aws apigateway create-resource --rest-api-id czpi202dx7 --parent-id gzhknt --path-part books 

   aws apigateway create-resource --rest-api-id czpi202dx7 --parent-id 7fxy31 --path-part "{id}"

   aws apigateway create-resource --rest-api-id czpi202dx7 --parent-id kf0r0r --path-part return

   aws apigateway create-resource --rest-api-id czpi202dx7 --parent-id kf0r0r --path-part checkout

  Below are the resources created within API Gateway

   /
   /api
   /api/books
   /api/books/{id}
   /api/books/{id}/return
   /api/books/{id}/checkout

-> Now we need to create a method (POST) under /api/books resource and integrate it with Lambda function created in the previous steps.

  aws apigateway get-resources --rest-api-id czpi202dx7

  aws apigateway put-method --rest-api-id czpi202dx7 --resource-id 7fxy31 --http-method POST --authorization-type NONE

  aws apigateway put-integration --rest-api-id czpi202dx7 --resource-id 7fxy31 --http-method POST --type AWS_PROXY --integration-http-method POST --uri arn:aws:apigateway:eu-north-1:lambda:path/2015-03-31/functions/arn:aws:lambda:eu-north-1:323432454298:function:booklendingserviceawssolution-lambdafunction/invocations

  aws lambda add-permission --function-name booklendingserviceawssolution-lambdafunction --statement-id apigateway-api-books --action lambda:InvokeFunction --principal apigateway.amazonaws.com --source-arn arn:aws:execute-api:eu-north-1:323432454298:einr441zf8/*/api/books

  aws apigateway create-deployment --rest-api-id czpi202dx7 --stage-name prod








 

