# ASP.NET Core WishList Application

The ASP.NET Core WishList Application is designed to allow users to create their own wishlists, and other users to mark that they are buying those items in such a way the owner of the wish list isn't able to see, while other users are able to see. This application is designed using the Model View Controller design pattern.

Note: This project is the second in a series of four projects, this project will cover taking an existing ASP.NET Core web application and changing it from only supporting one user to being able to support many users with authentication and basic security.

# Setup the Application

## If you want to use Visual Studio
If you want to use Visual Studio (highly recommended) follow the following steps:
-   If you already have Visual Studio installed make sure you have .Net Core installed by running the "Visual Studio Installer" and making sure ".NET Core cross-platform development" is checked
-   If you need to install visual studio download it at https://www.microsoft.com/net/download/ (If you'r using Windows you'll want to check "ASP.NET" and ".NET Core cross-platform development" on the workloads screen during installation.)
-   Open the .sln file in visual studio
-   To run the application simply press the Start Debug button (green arrow) or press F5
-   If you're using Visual Studio on Windows, to run tests open the Test menu, click Run, then click on Run all tests (results will show up in the Test Explorer)
-   If you're using Visual Studio on macOS, to run tests, select the GradeBookTests Project, then go to the Run menu, then click on Run Unit Tests (results will show up in the Unit Tests panel)

(Note: All tests should fail at this point, this is by design. As you progress through the projects more and more tests will pass. All tests should pass upon completion of the project.)

## If you don't plan to use Visual studio
If you would rather use something other than Visual Studio
-   Install the .Net Core SDK from https://www.microsoft.com/net/download/core once that installation completes you're ready to roll!
-   To run the application go into the GradeBook project folder and type `dotnet run`
-   To run the tests go into the GradeBookTests project folder and type `dotnet test`

# Features you will impliment

- Add support for user authentication
- Create functionality for creating and logging in
- Expand Wishlist functionality to support multiple users
- Impliment several basic security practices (validation tokens, user verification, authentication, etc)

## Tasks necessary to complete implimentation:

__Note:__ this isn't the only way to accomplish this, however; this is what the project's tests are expecting. Implimenting this in a different way will likely result in being marked as incomplete / incorrect.

- [ ] Adding Authentication to our existing ASP.NET Core App
	- [ ] Add Support for Authentication
		- [ ] Add AddIdentity to services Configuration
		- [ ] Add UseAuthentication to app Configure
	- [ ] 

	Change Existing

	Refactor / Reconfigure / Change
	- Change ApplicationDbContext Inherritance to IdentityDbContext instead of DbContext
	- Change _Layout to have Create / Login links if you're not logged in (forgot password?)
	- Change _Layout to have Logout link if you're logged in (change password?)
	- Change WishList Actions to require authentication
	- Change WishList Index Action to show only Items with logged in UserId
	- Change Item model to contain userId

	New Stuff

	Views to be created
	- Register
	- Login
	- Logout
	- Forgot password?
	- Change password?

	Controller to be created
	- Register Get
	- Register Post
	- Login Get
	- Login Post
	- Logout Get
	- Logout Post
	- Forgot Password Get?
	- Forgot Password Post?
	- Change Password Get?
	- Change Password Post?
	

## What Now?

You've compeleted the tasks of this project, if you want to continue working on this project there will be additional projects added to the ASP.NET Core path that continue where this project left off adding more advanced views and models, as well as providing and consuming data as a webservice.

Otherwise now is a good time to continue on the ASP.NET Core path to expand your understanding of the ASP.NET Core framework or take a look at the Microsoft Azure for Developers path as Azure is a common choice for hosting, scaling, and expanding the functionality of ASP.NET Core applications.