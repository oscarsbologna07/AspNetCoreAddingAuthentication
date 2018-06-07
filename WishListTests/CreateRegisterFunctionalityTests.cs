using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

            var pattern = @"</s*?form/s*.*?asp-action/s*?=/s*?""Register""/s*?.*?>";
            var rgx = new Regex(pattern);
            Assert.True(rgx.IsMatch(file), @"`Register.cshtml`'s `form` tag didn't contain an attribute `asp-action` set to ""Register"".");

            pattern = @"</s*?input/s*.*?asp-for/s*?=/s*?""Email""/s*?.*?[/]>";
            rgx = new Regex(pattern);
            Assert.True(rgx.IsMatch(file), @"`Register.cshtml` did not contain an `input` tag with an attribute `asp-for` set to ""Email"".");

            pattern = @"</s*?span/s*.*?asp-validation-for/s*?=/s*?""Email""/s*?.*?>";
            rgx = new Regex(pattern);
            Assert.True(rgx.IsMatch(file), @"`Register.cshtml` did not contain a `span` tag with an attribute `asp-validation-for` set to ""Email"".");

            pattern = @"</s*?input/s*.*?asp-for/s*?=/s*?""Password""/s*?.*?[/]>";
            rgx = new Regex(pattern);
            Assert.True(rgx.IsMatch(file), @"`Register.cshtml` did not contain an `input` tag with an attribute `asp-for` set to ""Password"".");

            pattern = @"</s*?span/s*.*?asp-validation-for/s*?=/s*?""Password""/s*?.*?>";
            rgx = new Regex(pattern);
            Assert.True(rgx.IsMatch(file), @"`Register.cshtml` did not contain a `span` tag with an attribute `asp-validation-for` set to ""Password"".");

            pattern = @"</s*?input/s*.*?asp-for/s*?=/s*?""ConfirmPassword""/s*?.*?[/]>";
            rgx = new Regex(pattern);
            Assert.True(rgx.IsMatch(file), @"`Register.cshtml` did not contain an `input` tag with an attribute `asp-for` set to ""ConfirmPassword"".");

            pattern = @"</s*?span/s*.*?asp-validation-for/s*?=/s*?""ConfirmPassword""/s*?.*?>";
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

            var userManeger = new UserManager<ApplicationUser>(null, null, null, null, null, null, null, null, null);
            var signInManager = new SignInManager<ApplicationUser>(null, null, null, null, null, null);
            var controller = Activator.CreateInstance(accountController, new object[] { userManeger, signInManager });
            var results = (ViewResult)method.Invoke(controller, null);
            Assert.True(results.ViewName == "Register");
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

            var userManeger = new UserManager<ApplicationUser>(null, null, null, null, null, null, null, null, null);
            var signInManager = new SignInManager<ApplicationUser>(null, null, null, null, null, null);
            var controller = Activator.CreateInstance(accountController, new object[] { userManeger, signInManager });
            var model = Activator.CreateInstance(registerViewModel, null);
            registerViewModel.GetProperty("Email").SetValue(model, "BadEmail");
            registerViewModel.GetProperty("Password").SetValue(model, "!4oOauidT_5");
            registerViewModel.GetProperty("ConfirmPassword").SetValue(model, "!4oOauidT_5");
            var badModelResults = (ViewResult)method.Invoke(controller, new object[] { model });
            Assert.True(badModelResults.ViewName == "Register", "`AccountController`'s Post `Register` method did not return the `Register` view when the `ModelState` was not valid.");
            Assert.True(badModelResults.Model == model, "`AccountController`'s Post `Register` method did not provide the invalid model when returning the `Register` view when the `ModelState` was not valid.");

            registerViewModel.GetProperty("Email").SetValue(model, "Valid@Email.com");
            var goodModelResults = (RedirectToActionResult)method.Invoke(controller, new object[] { model });
            Assert.True(goodModelResults.ControllerName == "Home" && goodModelResults.ActionName == "Index", "`AccountController`'s Post `Register` method did not return a `RedirectToAction` to the `Home.Index` action when a valid model was submitted.");
            // Need to verify user was created
        }
    }
}
