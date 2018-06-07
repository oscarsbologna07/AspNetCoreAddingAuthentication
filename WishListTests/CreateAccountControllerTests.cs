using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WishList.Models;
using Xunit;

namespace WishListTests
{
    public class CreateAccountControllerTests
    {
        [Fact(DisplayName = "Create AccountController @create-accountcontroller")]
        public void CreateAccountControllerTest()
        {
            var filePath = ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "WishList" + Path.DirectorySeparatorChar + "Controllers" + Path.DirectorySeparatorChar + "AccountController.cs";
            Assert.True(File.Exists(filePath), "`AccountController.cs` was not found in the `Controllers` folder.");

            var accountController = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                     from type in assembly.GetTypes()
                                     where type.FullName == "WishList.Controllers.AccountController"
                                     select type).FirstOrDefault();

            Assert.True(accountController != null, "A `public` class `AccountController` was not found in the `WishList.Controllers` namespace.");
            Assert.True(accountController.BaseType == typeof(Controller), "`AccountController` didn't inherit the `Controllers` class.");
            Assert.True(accountController.CustomAttributes.Where(e => e.AttributeType == typeof(AuthorizeAttribute)) != null, "`AccountController` didn't have an `Authorize` attribute.");
        }

        [Fact(DisplayName = "Create Private Fields @create-private-fields")]
        public void CreatePrivateFieldsTest()
        {
            var filePath = ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "WishList" + Path.DirectorySeparatorChar + "Controllers" + Path.DirectorySeparatorChar + "AccountController.cs";
            Assert.True(File.Exists(filePath), "`AccountController.cs` was not found in the `Controllers` folder.");

            var accountController = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                     from type in assembly.GetTypes()
                                     where type.FullName == "WishList.Controllers.AccountController"
                                     select type).FirstOrDefault();
            Assert.True(accountController != null, "A `public` class `AccountController` was not found in the `WishList.Controllers` namespace.");

            Assert.True(accountController.GetField("_userManager", System.Reflection.BindingFlags.NonPublic)?.FieldType == typeof(UserManager<ApplicationUser>), "`AccountController` does not appear to contain a `private` field `_userManager` of type `UserManager<ApplicationUser>`.");
            Assert.True(accountController.GetField("_signInManager", System.Reflection.BindingFlags.NonPublic)?.FieldType == typeof(SignInManager<ApplicationUser>), "AccountController` does not appear to contain a `private field `_signInManager` of type `SignInManager<ApplicationUser>`");
        }

        [Fact(DisplayName = "Create Constructor For AccountController @create-constructor-for-accountcontroller")]
        public void CreateConstructorForAccountControllerTest()
        {
            var filePath = ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "WishList" + Path.DirectorySeparatorChar + "Controllers" + Path.DirectorySeparatorChar + "AccountController.cs";
            Assert.True(File.Exists(filePath), "`AccountController.cs` was not found in the `Controllers` folder.");

            var accountController = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                     from type in assembly.GetTypes()
                                     where type.FullName == "WishList.Controllers.AccountController"
                                     select type).FirstOrDefault();
            Assert.True(accountController != null, "A `public` class `AccountController` was not found in the `WishList.Controllers` namespace.");

            var constructor = accountController.GetConstructors().FirstOrDefault();
            var parameters = constructor.GetParameters();
            Assert.True((parameters[0]?.GetType() == typeof(UserManager<ApplicationUser>) && parameters[1]?.GetType() == typeof(SignInManager<ApplicationUser>)), "`AccountController` did not contain a constructor with two parameters, first of type `UserManager<ApplicationUser>`, second of type `SignInManager<ApplicationUser>`.");

            var userManager = new UserManager<ApplicationUser>(null, null, null, null, null, null, null, null, null);
            var signInManager = new SignInManager<ApplicationUser>(userManager, null, null, null, null, null);
            var controller = Activator.CreateInstance(accountController, new object[] { userManager, signInManager });
            Assert.True(accountController.GetField("_userManager", System.Reflection.BindingFlags.NonPublic)?.GetValue(controller) != null, "`AccountController`'s constructor did not set the `_userManager` field based on the provided `UserManager<ApplicationUser>` parameter.");
            Assert.True(accountController.GetField("_signInManager", System.Reflection.BindingFlags.NonPublic)?.GetValue(controller) != null, "`AccountController``s constructor did not set the `_signInManager` field based on the provided `SignInManager<ApplicationUser>` parameter.");
        }
    }
}
