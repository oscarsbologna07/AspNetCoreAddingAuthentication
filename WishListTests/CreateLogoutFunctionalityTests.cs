using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WishList.Models;
using Xunit;

namespace WishListTests
{
    public class CreateLogoutFunctionalityTests
    {
        [Fact(DisplayName = "Create Logout Action @create-logout-action")]
        public async void CreateHttpPostLogoutActionTest()
        {
            var filePath = ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "WishList" + Path.DirectorySeparatorChar + "Controllers" + Path.DirectorySeparatorChar + "AccountController.cs";
            Assert.True(File.Exists(filePath), @"`AccountController.cs` was not found in the `Controllers` folder.");

            var accountController = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                     from type in assembly.GetTypes()
                                     where type.FullName == "WishList.Controllers.AccountController"
                                     select type).FirstOrDefault();
            Assert.True(accountController != null, "A `public` class `AccountController` was not found in the `WishList.Controllers` namespace.");

            var method = accountController.GetMethod("Logout", new Type[] { });
            Assert.True(method != null, "`AccountController` did not contain a `Logout` method with a parameter of type `LogoutViewModel`.");
            Assert.True(method.ReturnType == typeof(IActionResult), "`AccountController`'s Post `Logout` method did not have a return type of `IActionResult`");
            Assert.True(method.CustomAttributes.FirstOrDefault(e => e.AttributeType == typeof(HttpPostAttribute)) != null, "`AccountController``s `Logout` method did not have the `HttpPost` attribute.");
            Assert.True(method.CustomAttributes.FirstOrDefault(e => e.AttributeType == typeof(ValidateAntiForgeryTokenAttribute)) != null, "`AccountController`'s `Logout` method did not have the `ValidateAntiForgeryToken` attribute.");
            // Note: Attribute AsyncStateMachine can be used to test for the presence of the `async` keyword as it should only exist on methods with the `async` keyword
            Assert.True(method.CustomAttributes.FirstOrDefault(e => e.AttributeType == typeof(AsyncStateMachineAttribute)) != null, "`AccountController`'s `Logout` method did not have the keyword `async` in it's signature.");
            Assert.True(method.ReturnType == typeof(Task<IActionResult>), "`AccountController`'s `Logout` method did not have a return type of `Task<IActionResult>`.");

            var userManeger = new UserManager<ApplicationUser>(null, null, null, null, null, null, null, null, null);
            var signInManager = new SignInManager<ApplicationUser>(null, null, null, null, null, null);
            var controller = Activator.CreateInstance(accountController, new object[] { userManeger, signInManager });
            var results = await (dynamic)method.Invoke(controller, new object[] { });
            Assert.True(results.ControllerName == "Home" && results.ActionName == "Index", "`AccountController`'s `Logout` method did not return a `RedirectToAction` to the `Home.Index` action when logout was successful.");
        }
    }
}
