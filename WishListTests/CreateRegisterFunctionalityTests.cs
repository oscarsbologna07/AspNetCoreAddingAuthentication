using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            Assert.True(emailProperty.CustomAttributes.FirstOrDefault(e => e.AttributeType == typeof(RequiredAttribute)) != null, "`RegisterViewModel` has a property `Email` but it doesn't appear to have a `Required` attribute.");
            Assert.True(emailProperty.CustomAttributes.FirstOrDefault(e => e.AttributeType == typeof(EmailAddressAttribute)) != null,"`RegisterViewModel` has a property `Email` but it doesn't appear to have an `EmailAddress` attribute.");

            var passwordProperty = registerViewModel.GetProperty("Password");
            Assert.True(passwordProperty != null, "`RegisterViewModel` does not appear to contain a `public` `string` property `Password`.");
            Assert.True(passwordProperty.PropertyType == typeof(string), "`RegisterViewModel` has a property `Password` but it is not of type `string`.");
            Assert.True(passwordProperty.CustomAttributes.FirstOrDefault(e => e.AttributeType == typeof(RequiredAttribute)) != null, "`RegisterViewModel` has a property `Password` but it doesn't appear to have a `Required` attribute.");
            Assert.True(passwordProperty.CustomAttributes.FirstOrDefault(e => e.AttributeType == typeof(StringLengthAttribute)) != null, "`RegisterViewModel` has a property `Password` but it doesn't appear to have a `StringLength` attribute of 100, with a minimum length of 8");
            // need to verify string length's max and minlength
            Assert.True(passwordProperty.CustomAttributes.FirstOrDefault(e => e.AttributeType == typeof(DataTypeAttribute)) != null, "`RegisterViewModel` has a property `Password` but it doesn't appear to have a `DataType` attribute set to `Password`.");
            // need to verify datatype is set to password

            var confirmPasswordProperty = registerViewModel.GetProperty("ConfirmPassword");
            Assert.True(confirmPasswordProperty != null, "`RegisterViewModel` does not appear to contain a `public` `string` property `ConfirmPassword`.");
            Assert.True(confirmPasswordProperty.PropertyType == typeof(string), "`RegisterViewModel` has a property `ConfirmPassword` but it is not of type `string`.");
            Assert.True(confirmPasswordProperty.CustomAttributes.FirstOrDefault(e => e.AttributeType == typeof(DataTypeAttribute)) != null, "`RegisterViewModel` has a property `ConfirmPassword` but it doesn't appear to have a `DataType` attribute set to `Password`.");
            // need to verify datatype is set to password
            Assert.True(confirmPasswordProperty.CustomAttributes.FirstOrDefault(e => e.AttributeType == typeof(CompareAttribute)) != null, "`RegisterViewModel` has a property `ConfirmPassword` but it doesn't appeart to have a `Compare` attribute set to `Password`.");
            // need to verify compare is set to password
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
            var method = accountController.GetMethod("Register", new Type[]{});
            Assert.True(method != null, "`AccountController` did not contain a `public` `Register` method with no parameters");
            Assert.True(method.ReturnType == typeof(IActionResult), "`AccountController`'s Get `Register` method did not have a return type of `IActionResult`");
            Assert.True(method.CustomAttributes.FirstOrDefault(e => e.AttributeType == typeof(HttpGetAttribute)) != null, "`AccountController` did not contain a `Register` method with an `HttpGet` attribute");
            Assert.True(method.CustomAttributes.FirstOrDefault(e => e.AttributeType == typeof(AllowAnonymousAttribute)) != null, "`AccountController`'s Get `Register` method did not have the `AllowAnonymous` attribute");

            string file;
            using (var streamReader = new StreamReader(filePath))
            {
                file = streamReader.ReadToEnd();
            }

            var pattern = @"return\s*View[(]/s*?(""Register"")?/s*?[)];";
            var rgx = new Regex(pattern);
            Assert.True(rgx.IsMatch(file), @"`AccountController`'s Get `Register` action did not return the `Register` view.");
            // Need to verify this runs on the correct method
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

            string file;
            using (var streamReader = new StreamReader(filePath))
            {
                file = streamReader.ReadToEnd();
            }

            var pattern = @"return\s*View[(]/s*?(""Register"")?/s*?[)];";
            var rgx = new Regex(pattern);
            Assert.True(rgx.IsMatch(file), @"`AccountController`'s Post `Register` action did not return the `Register` view.");
            // Need to verify this is running on the correct method
        }
    }
}
