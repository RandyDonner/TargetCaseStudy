# TargetCaseStudy
Repository for sharing source code with Target.

Task: myRetail RESTful service

I made a C# ASP.NET WebAPI for this project. I used the default template so there is also a default MVC site with it. Included in the solution is a unit test framework but I will also demo and can provide postman examples upon request. This project has unit test coverage running at 100% as well as successful functionality with postman.

Right out of the box you are able to get the nuget packages and run the unit tests. Run GetById first to add a record to the liteDb and then all tests will pass. Running on local host you are able to use postman to hit the endpoints. The put request needs to have the id in the URL and the value in the body as json.

URL as well as LiteDb configuration in web.config for API and app.config for unit test. LiteDb set to c:\myRetail. When running as admin it was able to create the folder for me but if unable just manually add the folder and it should work.

Example endpoints:
1. GET localhost:50124/api/products/13860428
2. GET localhost:50124/api/products
3. PUT localhost:50124/api/products/putValue/13860428 raw json body: 
{
	"value": "7.15"
}

When reviewing the code I tried to add comments but the main classes to look at are:
1. ProductsController
2. Product
3. ExternalAPIInformationGather
4. ProductsControllerTest

I made sure to double check that all todo's on the task have been completed as well as the BONUS.

Let me know if there is anything more you would like for me to explain or go over.

Thanks again!
