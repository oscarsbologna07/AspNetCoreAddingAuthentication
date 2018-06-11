using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using WishList.Controllers;
using WishList.Models;
using Xunit;

namespace WishListTests
{
    public class CreateRegisterFunctionalityTests
    {
        [Fact(DisplayName = "Create Register Model @create-register-model")]
        public void CreateRegisterModel()
        {
            var filePath = ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "WishList" + Path.DirectorySeparatorChar + "Models" + Path.DirectorySeparatorChar + "AccountViewModels" + Path.DirectorySeparatorChar + "RegisterViewModel.cs";
            Assert.True(File.Exists(filePath), @"`RegisterViewModel.cs` was not found in the `Models\AccountViewModels` folder.");

            var registerViewModel = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                     from type in assembly.GetTypes()
                                     where type.FullName == "WishList.Models.AccountViewModels.RegisterViewModel"
                                     select type).FirstOrDefault();
            Assert.True(registerViewModel != null, "A `public` class `RegisterViewModel` was not found in the `WishList.Models.AccountViewModels` namespace.");

            var emailProperty = registerViewModel.GetProperty("Email");
            Assert.True(emailProperty != null, "`RegisterViewModel` does not appear to contain a `public` `string` property `Email`.");
            Assert.True(emailProperty.PropertyType == typeof(string), "`RegisterViewModel` has a property `Email` but it is not of type `string`.");
            Assert.True(Attribute.IsDefined(emailProperty, typeof(RequiredAttribute)), "`RegisterViewModel` has a property `Email` but it doesn't appear to have a `Required` attribute.");
            Assert.True(emailProperty.CustomAttributes.FirstOrDefault(e => e.AttributeType == typeof(EmailAddressAttribute)) != null, "`RegisterViewModel` has a property `Email` but it doesn't appear to have an `EmailAddress` attribute.");

            var passwordProperty = registerViewModel.GetProperty("Password");
            Assert.True(passwordProperty != null, "`RegisterViewModel` does not appear to contain a `public` `string` property `Password`.");
            Assert.True(passwordProperty.PropertyType == typeof(string), "`RegisterViewModel` has a property `Password` but it is not of type `string`.");
            Assert.True(passwordProperty.CustomAttributes.FirstOrDefault(e => e.AttributeType == typeof(RequiredAttribute)) != null, "`RegisterViewModel` has a property `Password` but it doesn't appear to have a `Required` attribute.");
            var stringLength = passwordProperty.CustomAttributes.FirstOrDefault(e => e.AttributeType == typeof(StringLengthAttribute));
            Assert.True(stringLength != null, "`RegisterViewModel` has a property `Password` but it doesn't appear to have a `StringLength` attribute of 100, with a minimum length of 8");
            Assert.True(stringLength.ConstructorArguments.Count == 1 && (int)stringLength.ConstructorArguments[0].Value == 100, "`RegisterViewModel` has a property `Password`and a `StringLength` attribute, but it's `maxLength` isn't set to 100.");
            Assert.True(stringLength.NamedArguments.Count == 1 && (int)stringLength.NamedArguments[0].TypedValue.Value == 8, "`RegisterViewModel` has a property `Password` but it doesn't appear to have a `StringLength` attribute of 100, but it's minimum length was not set to 8");
            var dataType = passwordProperty.CustomAttributes.FirstOrDefault(e => e.AttributeType == typeof(DataTypeAttribute));
            Assert.True(dataType != null, "`RegisterViewModel` has a property `Password` but it doesn't appear to have a `DataType` attribute set to `Password`.");
            Assert.True(dataType.ConstructorArguments.Count == 1 && (int)dataType.ConstructorArguments[0].Value == 11, "`RegisterViewModel` has a property `Password` and it has a `DataType` attribute but it's not set to `DataType.Password`.");

            var confirmPasswordProperty = registerViewModel.GetProperty("ConfirmPassword");
            Assert.True(confirmPasswordProperty != null, "`RegisterViewModel` does not appear to contain a `public` `string` property `ConfirmPassword`.");
            Assert.True(confirmPasswordProperty.PropertyType == typeof(string), "`RegisterViewModel` has a property `ConfirmPassword` but it is not of type `string`.");
            dataType = confirmPasswordProperty.CustomAttributes.FirstOrDefault(e => e.AttributeType == typeof(DataTypeAttribute));
            Assert.True(dataType != null, "`RegisterViewModel` has a property `ConfirmPassword` but it doesn't appear to have a `DataType` attribute set to `DataType.Password`.");
            Assert.True(dataType.ConstructorArguments.Count == 1 && (int)dataType.ConstructorArguments[0].Value == 11, "`RegisterViewModel` has a property `ConfirmPassword` and it has a `DataType` attribute but it's not set to `DataType.Password`.");
            var confirm = confirmPasswordProperty.CustomAttributes.FirstOrDefault(e => e.AttributeType == typeof(CompareAttribute));
            Assert.True(confirm != null, "`RegisterViewModel` has a property `ConfirmPassword` but it doesn't appear to have a `Compare` attribute set to `Password`.");
            Assert.True(confirm.ConstructorArguments.Count == 1 && (string)confirm.ConstructorArguments[0].Value == "Password", "`RegisterViewModel` has a property `ConfirmPassword` with a `Compare` attribute, but it is not set to `Password`.");
        }

        [Fact(DisplayName = "Create Register View @create-register-view")]
        public void CreateRegisterViewTest()
        {
            var filePath = ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "WishList" + Path.DirectorySeparatorChar + "Views" + Path.DirectorySeparatorChar + "Account" + Path.DirectorySeparatorChar + "Register.cshtml";
            Assert.True(File.Exists(filePath), @"`Register.cshtml` was not found in the `Views\Account` folder.");

            string file;
            using (var streamReader = new StreamReader(filePath))
            {
                file = streamReader.ReadToEnd();
            }

            var pattern = @"@model\s*WishList[.]Models[.]AccountViewModels[.]RegisterViewModel";
            var rgx = new Regex(pattern);
            Assert.True(rgx.IsMatch(file), @"`Register.cshtml` doesn't have it's model set to `WishList.Models.AccountViewModels.RegisterViewModel`.");

            pattern = @"<\s*?form\s*.*?asp-action\s*?=\s*?""Register""\s*?.*?>";
            rgx = new Regex(pattern);
            Assert.True(rgx.IsMatch(file), @"`Register.cshtml`'s `form` tag didn't contain an attribute `asp-action` set to ""Register"".");

            pattern = @"<\s*?input\s*.*?asp-for\s*?=\s*?""Email""\s*?.*?[/]>";
            rgx = new Regex(pattern);
            Assert.True(rgx.IsMatch(file), @"`Register.cshtml` did not contain an `input` tag with an attribute `asp-for` set to ""Email"".");

            pattern = @"<\s*?span\s*.*?asp-validation-for\s*?=\s*?""Email""\s*?.*?>";
            rgx = new Regex(pattern);
            Assert.True(rgx.IsMatch(file), @"`Register.cshtml` did not contain a `span` tag with an attribute `asp-validation-for` set to ""Email"".");

            pattern = @"<\s*?input\s*.*?asp-for\s*?=\s*?""Password""\s*?.*?[/]>";
            rgx = new Regex(pattern);
            Assert.True(rgx.IsMatch(file), @"`Register.cshtml` did not contain an `input` tag with an attribute `asp-for` set to ""Password"".");

            pattern = @"<\s*?span\s*.*?asp-validation-for\s*?=\s*?""Password""\s*?.*?>";
            rgx = new Regex(pattern);
            Assert.True(rgx.IsMatch(file), @"`Register.cshtml` did not contain a `span` tag with an attribute `asp-validation-for` set to ""Password"".");

            pattern = @"<\s*?input\s*.*?asp-for\s*?=\s*?""ConfirmPassword""\s*?.*?[/]>";
            rgx = new Regex(pattern);
            Assert.True(rgx.IsMatch(file), @"`Register.cshtml` did not contain an `input` tag with an attribute `asp-for` set to ""ConfirmPassword"".");

            pattern = @"<\s*?span\s*.*?asp-validation-for\s*?=\s*?""ConfirmPassword""\s*?.*?>";
            rgx = new Regex(pattern);
            Assert.True(rgx.IsMatch(file), @"`Register.cshtml` did not contain a `span` tag with an attribute `asp-validation-for` set to ""ConfirmPassword"".");
        }

        [Fact(DisplayName = "Create HttpGet Register Action @create-httpget-register-action")]
        public void CreateHttpGetRegisterActionTest()
        {
            var filePath = ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "WishList" + Path.DirectorySeparatorChar + "Controllers" + Path.DirectorySeparatorChar + "AccountController.cs";
            Assert.True(File.Exists(filePath), @"`AccountController.cs` was not found in the `Controllers` folder.");

            var accountController = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                     from type in assembly.GetTypes()
                                     where type.FullName == "WishList.Controllers.AccountController"
                                     select type).FirstOrDefault();
            Assert.True(accountController != null, "A `public` class `AccountController` was not found in the `WishList.Controllers` namespace.");
            var method = accountController.GetMethod("Register", new Type[] { });
            Assert.True(method != null, "`AccountController` did not contain a `public` `Register` method with no parameters");
            Assert.True(method.ReturnType == typeof(IActionResult), "`AccountController`'s Get `Register` method did not have a return type of `IActionResult`");
            Assert.True(method.CustomAttributes.FirstOrDefault(e => e.AttributeType == typeof(HttpGetAttribute)) != null, "`AccountController` did not contain a `Register` method with an `HttpGet` attribute");
            Assert.True(method.CustomAttributes.FirstOrDefault(e => e.AttributeType == typeof(AllowAnonymousAttribute)) != null, "`AccountController`'s Get `Register` method did not have the `AllowAnonymous` attribute");

            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            var userManager = new UserManager<ApplicationUser>(userStore.Object, null, null, null, null, null, null, null, null);
            var signInManager = new SignInManager<ApplicationUser>(userManager, contextAccessor.Object, claimsFactory.Object, null, null, null);
            var controller = Activator.CreateInstance(accountController, new object[] { userManager, signInManager });
            var results = method.Invoke(controller, null) as ViewResult;
            Assert.True(results != null, "`AccountController`'s HttpGet `Register` action did not return a the `Register` view.");
            Assert.True(results.ViewName == "Register" || results.ViewName == null, "`AccountController`'s HttpGet `Register` action did not return a the `Register` view.");
        }

        [Fact(DisplayName = "Create HttpPost Register Action @create-httppost-register-action")]
        public void CreateHttpPostRegisterActionTest()
        {
            var filePath = ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "WishList" + Path.DirectorySeparatorChar + "Controllers" + Path.DirectorySeparatorChar + "AccountController.cs";
            Assert.True(File.Exists(filePath), @"`AccountController.cs` was not found in the `Controllers` folder.");

            var accountController = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                     from type in assembly.GetTypes()
                                     where type.FullName == "WishList.Controllers.AccountController"
                                     select type).FirstOrDefault();
            Assert.True(accountController != null, "A `public` class `AccountController` was not found in the `WishList.Controllers` namespace.");

            var registerViewModel = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                     from type in assembly.GetTypes()
                                     where type.FullName == "WishList.Models.AccountViewModels.RegisterViewModel"
                                     select type).FirstOrDefault();
            Assert.True(registerViewModel != null, "A `public` class `RegisterViewModel` was not found in the `WishList.Models.AccountViewModels` namespace.");

            var method = accountController.GetMethod("Register", new Type[] { registerViewModel });
            Assert.True(method != null, "`AccountController` did not contain a `Register` method with a parameter of type `RegisterViewModel`.");
            Assert.True(method.ReturnType == typeof(IActionResult), "`AccountController`'s Post `Register` method did not have a return type of `IActionResult`");
            Assert.True(method.CustomAttributes.FirstOrDefault(e => e.AttributeType == typeof(HttpPostAttribute)) != null, "`AccountController` did not contain a `Register` method with an `HttpPost` attribute");
            Assert.True(method.CustomAttributes.FirstOrDefault(e => e.AttributeType == typeof(AllowAnonymousAttribute)) != null, "`AccountController`'s Post `Register` method did not have the `AllowAnonymous` attribute");

            var userStore = new Mock<IUserPasswordStore<ApplicationUser>>();
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(e => e.CreateAsync(It.IsAny<ApplicationUser>(),It.IsAny<string>())).ReturnsAsync(new IdentityResult()).Verifiable();
            var signInManager = new Mock<SignInManager<ApplicationUser>>(userManager.Object, contextAccessor.Object, claimsFactory.Object, null, null, null);
            var controller = Activator.CreateInstance(accountController, new object[] { userManager.Object, signInManager.Object });
            var model = Activator.CreateInstance(registerViewModel, null);
            registerViewModel.GetProperty("Email").SetValue(model, "Test@Test.com");
            registerViewModel.GetProperty("Password").SetValue(model, "Aeoi89a8#$@aou");
            registerViewModel.GetProperty("ConfirmPassword").SetValue(model, "Aeoi89a8#$@aou");

            var goodModelResults = method.Invoke(controller, new object[] { model }) as RedirectToActionResult;
            try
            {
                userManager.Verify();
            }
            catch (MockException)
            {
                Assert.True(false, "`AccountController`'s Post `Register` action did not create a new user when the model was valid.");
            }
            Assert.True(goodModelResults != null && goodModelResults.ControllerName == "Home" && goodModelResults.ActionName == "Index", "`AccountController`'s Post `Register` method did not return a `RedirectToAction` to the `Home.Index` action when a valid model was submitted.");

            var modelState = accountController.GetProperty("ModelState").GetValue(controller);
            var addModelError = typeof(ModelStateDictionary).GetMethod("AddModelError", new Type[] { typeof(string), typeof(string) });
            addModelError.Invoke(modelState, new object[] { "Email", "The entered email is not a valid email address." });

            var badModelResults = method.Invoke(controller, new object[] { model }) as ViewResult;
            Assert.True(badModelResults != null && (badModelResults.ViewName == "Register" || badModelResults.ViewName == null), "`AccountController`'s Post `Register` method did not return the `Register` view when the `ModelState` was not valid.");
            Assert.True(badModelResults.Model == model, "`AccountController`'s Post `Register` method did not provide the invalid model when returning the `Register` view when the `ModelState` was not valid.");
        }
    }
}
