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
-   To run the application go into the WishList project folder and type `dotnet run`
-   To run the tests go into the WishListTests project folder and type `dotnet test`

# Features you will impliment

- Add support for user authentication
- Create functionality for creating and logging in
- Expand Wishlist functionality to support multiple users
- Impliment several basic security practices (validation tokens, user verification, authentication, etc)

## Tasks necessary to complete implimentation:

__Note:__ this isn't the only way to accomplish this, however; this is what the project's tests are expecting. Implimenting this in a different way will likely result in being marked as incomplete / incorrect.

- [ ] Adding Authentication to our existing ASP.NET Core wishlist app
	- [ ] Create `ApplicationUser` Model
		- [ ] Inside the `Models` folder create a new model `ApplicationUser`
			- `ApplicationUser` should inherit `IdentityUser` class (you will need a `using` statement for `Microsoft.AspNetCore.Identity`)
	- [ ] Change `ApplicationDbContext` to use `IdentityDbContext`
		- [ ] Replace `AppicationDbContext`'s inherritance of `DbContext` to `IdentityDbContext<ApplicationUser>`
	- [ ] Add Support for `Authentication`
		- [ ] In the `Configuration` call `AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();` on `services`
		- [ ] In the `Configure` method Before `app.UseMvcWithDefaultRoute();` call `UseAuthentication` on `app`.
	- [ ] Create `AccountController`
		- [ ] Create new controller `AccountController` in the `Controllers` folder
			- The AccountController class should have the `authorize` attribute
			- This should inherit the `Controller` class (you will need a `using` statement for `Microsoft.AspNetCore.Mvc`)
		- [ ] Create private fields
			- This should have a private readonly field of type `UserManager<ApplicationUser>` named `_userManager`
			- This should have a private readonly field of type `SignInManager<ApplicationUser>` named `_signInManager`
		- [ ] Create `AccountController` Constructor
			- This constructor should accept two parameters, the first of type `UserManager<ApplicationUser>`, the second of type `signInManager<ApplicationUser>`
			- This constructor should set each of the private readonly properties using the parameter of the same type
		- [ ] Add public property
			- This should have a public property of type `string` named `ErrorMessage` (both the getter and setter should be public)
	- [ ] Create Register Functionality
		- [ ] Create Register Model
			- Inside the `Models/AccountViewModels` folder create a new model `RegisterViewModel` (You will need to create the AccountViewModels folder)
			- Create a `String` Property Email
				- Email should have the Required attribute
				- Email should have the EmailAddress attribute
			- Create a String Property `Password`
				- `Password` should have the `Required` attribute
				- `Password` should have the `DataType.Password` attribute
				- `Password` should have a `StringLength` attribute of 100 with a `MinLength` of 8 characters
			- Create a `String` Property `ConfirmPassword`
				- `ConfirmPassword` should have the `Required` attribute
				- `ConfirmPassword` should have the `DataType.Password` attribute
				- `ConfirmPassword` should have the `Compare` attribute with "Password"
		- [ ] Create the Register View
			- Inside the `Views/Account` folder add a new view `Register` (you will need to create the `Account` folder)
			- `Register` should have a model of `RegisterViewModel`
			- Add the following HTML to the view (we're providing this to save you from needing to type it all yourself)
				```
				<h3>Register New User</h3>
				<form>
					<input />
					<span></span>
					<input />
					<span></span>
					<input />
					<span></span>
					<button type="sumbit">Register User</button>
				</form>
				```
			- Add an attribute `asp-action` with a value of `"Register" to the `form` tag
			- Add an attribute `asp-for` with a value of `"Email"` to the first `input` tag
			- Add an attribute `asp-validation-for` with a value of `"Email"` to the first `span` tag
			- Add an attribute `asp-for` with a value of `"Password"` to the second `input` tag
			- Add an attribute `asp-validation-for` with a value of `"Password"` to the second `span` tag
			- Add an attribute `asp-for` with a value of `"ConfirmPassword"` to the third `input` tag
			- Add an attribute `asp-validation-for` with a value of `"ConfirmPassword"` to the third `span` tag
		- [ ] Create an HttpGet Action Register in the AccountController
			- This action should have the `HttpGet` attribute
			- This action should have the `AllowAnonymous` attribute
			- This action should have no parameters
			- This action should explicitly return the `Register` view.
		- [ ] Create an HttpPost Action Register in the AccountController
			- This action should have the `HttpPost` attribute
			- This action should accept a parameter of type `RegisterViewModel`
			- This action should check if the `ModelState` is valid
				- If so create the new user the redirect to action to the `Home.Index` action
				- If not return the `Register` view with the model provided parameter as it's model
	- [ ] Create Login Functionality
		- [ ] Create Login Model
			- Create a `String` Property Email
				- Email should have the Required attribute
				- Email should have the EmailAddress attribute
			- Create a String Property `Password`
				- `Password` should have the `Required` attribute
				- `Password` should have the `DataType.Password` attribute
				- `Password` should have a `StringLength` attribute of 100 with a `MinLength` of 8 characters
		- [ ] Create a Login View in the `Views/Account` Folder
			- Add the following HTML to the `Login` view
				```
				@model WishList.Models.LoginViemModel
				<h2>Log in</h2>
				<form asp-action="Login" method="post">
					<div>
						<label asp-for="Email"></label>
						<input asp-for="Email"/>
						<span asp-validation-for="Email"></span>
					</div>
					<div>
						<label asp-for="Password"></label>
						<input asp-for="Password" />
						<span asp-validation-for="Password"></span>
					</div>
					<div>
						<button type="submit">Log in</button>
					</div>
				</form>
				```
		- [ ] Create an HttpPost Action Login in the AccountController
			- This action should have the `HttpPost` attribute
			- This action should have the `AllowAnnonymoust` attribute
			- This action should have the `ValidateAntiForgeryToken` attribute
			- This action should use the `async` keyword
			- This action should have a return type of `Task<IActionResult>`
			- This action should accept a parameter of type `LoginViewModel`
			- This should use `SignInManager`'s `PasswordSignInAsync` method to attempt to login the user (Note: you will need to `await` or get the `Result` from async methods to see the results)
				- If the result is `Succeeded` return a `RedirectToAction` to the `Wishlist.Index` action
				- Otherwise use `ModelState`'s `AddModelError` with a key of `string.Empty` and an `errorMessage` of "Invalid login attempt."
	- [ ] Create Logout Functionality
		- [ ] Create an HttpPost Action Logout in the AcctionController
			- This action should have the `HttpPost` attribute
			- This action should have the `Authorize` attribute
			- This action should have the `ValidateAntiForgeryToken` attribute
			- This action should use the `async` keyword
			- This action should have a return type of `Task<IActionResult>`
			- This action should use `SignInManager`'s `SignOutAsync` method (Note: you will want to `await` this method to ensure the sign out completes)
			- This should return a `RedirectToAction` to the `Home.Index` action
	- [ ] Update Item Model to Include UserId -- Come back to this to ensure this is done according to new best practices
		- [ ] Add UserId to Item model
			- UserId should have Required attribute
	- [ ] Update ItemController Actions to consider UserId
		- [ ] Add the `Authorize` attribute to the `ItemController` class
		- [ ] Update the `ItemController.Index` action
			- Change the `_context.Items.ToList();` to include a `Where` call that only gets items with the matching userId.
		- [ ] Update `ItemController.Create` (HttpPost action) action
			- Add a line before adding the Item to set the UserId to the logged in user's Id
	
## What Now?

You've compeleted the tasks of this project, if you want to continue working on this project there will be additional projects added to the ASP.NET Core path that continue where this project left off adding more advanced views and models, as well as providing and consuming data as a webservice.

Otherwise now is a good time to continue on the ASP.NET Core path to expand your understanding of the ASP.NET Core framework or take a look at the Microsoft Azure for Developers path as Azure is a common choice for hosting, scaling, and expanding the functionality of ASP.NET Core applications.