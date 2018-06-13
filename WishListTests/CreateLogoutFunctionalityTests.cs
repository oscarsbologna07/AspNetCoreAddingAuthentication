using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WishList.Models;
using Xunit;

namespace WishListTests
{
    public class CreateLogoutFunctionalityTests
    {
        [Fact(DisplayName = "Create Logout Action @create-logout-action")]
        public void CreateHttpPostLogoutActionTest()
        {
            var filePath = ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "WishList" + Path.DirectorySeparatorChar + "Controllers" + Path.DirectorySeparatorChar + "AccountController.cs";
            Assert.True(File.Exists(filePath), @"`AccountController.cs` was not found in the `Controllers` folder.");

            var accountController = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                     from type in assembly.GetTypes()
                                     where type.FullName == "WishList.Controllers.AccountController"
                                     select type).FirstOrDefault();
            Assert.True(accountController != null, "A `public` class `AccountController` was not found in the `WishList.Controllers` namespace.");

            var method = accountController.GetMethod("Logout", new Type[] { });
            Assert.True(method != null, "`AccountController` did not contain a `Logout` method.");
            Assert.True(method.ReturnType == typeof(IActionResult), "`AccountController`'s Post `Logout` method did not have a return type of `IActionResult`");
            Assert.True(method.CustomAttributes.FirstOrDefault(e => e.AttributeType == typeof(HttpPostAttribute)) != null, "`AccountController``s `Logout` method did not have the `HttpPost` attribute.");
            Assert.True(method.CustomAttributes.FirstOrDefault(e => e.AttributeType == typeof(ValidateAntiForgeryTokenAttribute)) != null, "`AccountController`'s `Logout` method did not have the `ValidateAntiForgeryToken` attribute.");

            var userStore = new Mock<IUserPasswordStore<ApplicationUser>>();
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            var signInManager = new Mock<SignInManager<ApplicationUser>>(userManager.Object, contextAccessor.Object, claimsFactory.Object, null, null, null);
            signInManager.Setup(e => e.SignOutAsync()).Verifiable();
            var controller = Activator.CreateInstance(accountController, new object[] { userManager.Object, signInManager.Object });
            var results = method.Invoke(controller, new object[] { }) as RedirectToActionResult;
            try
            {
                signInManager.Verify();
            }
            catch (MockException)
            {
                Assert.True(false, "`AccountController`'s Post `Logout` action did not attempt to login out the user using `SignOutAsync`.");
            }
            Assert.True(results != null && results.ControllerName == "Home" && results.ActionName == "Index", "`AccountController`'s `Logout` method did not return a `RedirectToAction` to the `Home.Index` action.");
        }
    }
}
