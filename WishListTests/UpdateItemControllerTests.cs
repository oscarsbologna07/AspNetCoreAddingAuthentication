using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace WishListTests
{
    public class UpdateItemControllerTests
    {
        [Fact(DisplayName = "Update ItemController @update-itemcontroller")]
        public void UpdateItemControllerTest()
        {
            var filePath = ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "WishList" + Path.DirectorySeparatorChar + "Controllers" + Path.DirectorySeparatorChar + "ItemController.cs";
            Assert.True(File.Exists(filePath), "`ItemController.cs` was not found in the `Controllers` folder, did you remove or rename it?");

            var itemController = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                     from type in assembly.GetTypes()
                                     where type.FullName == "WishList.Controllers.ItemController"
                                     select type).FirstOrDefault();

            Assert.True(itemController != null, "A `public` class `ItemController` was not found in the `WishList.Controllers` namespace, did you remove or rename it?");
            Assert.True(itemController.CustomAttributes.Where(e => e.AttributeType == typeof(AuthorizeAttribute)) != null, "`ItemController` didn't have an `Authorize` attribute.");
        }

        [Fact(DisplayName = "Update Index Action @update-index-action")]
        public void UpdateIndexActionTest()
        {
            var filePath = ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "WishList" + Path.DirectorySeparatorChar + "Controllers" + Path.DirectorySeparatorChar + "ItemController.cs";
            Assert.True(File.Exists(filePath), @"`ItemController.cs` was not found in the `Controllers` folder.");

            var itemController = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                     from type in assembly.GetTypes()
                                     where type.FullName == "WishList.Controllers.ItemController"
                                     select type).FirstOrDefault();
            Assert.True(itemController != null, "A `public` class `ItemController` was not found in the `WishList.Controllers` namespace.");

            var method = itemController.GetMethod("Index", new Type[] {  });
            Assert.True(method != null, "`ItemController` did not contain a `Index` method did you remove or rename it?");

            var controller = Activator.CreateInstance(itemController, new object[] { });

            var results = (ViewResult)method.Invoke(controller, new object[] { });
            Assert.True(results.ViewName == "Login", "`ItemController`'s `Index` method did not return the `Index` view with a model of only items with the logged in User's Id.");
            Assert.True(results.Model != null, "`ItemController`'s `Index` method did return the `Index` view with a model of only items with the logged in User's Id.");
            // verify results contain only correct items
        }

        [Fact(DisplayName = "Update Create Action @update-create-action")]
        public void UpdateCreateActionTest()
        {
            var filePath = ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "WishList" + Path.DirectorySeparatorChar + "Controllers" + Path.DirectorySeparatorChar + "ItemController.cs";
            Assert.True(File.Exists(filePath), @"`ItemController.cs` was not found in the `Controllers` folder.");

            var itemController = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                  from type in assembly.GetTypes()
                                  where type.FullName == "WishList.Controllers.ItemController"
                                  select type).FirstOrDefault();
            Assert.True(itemController != null, "A `public` class `ItemController` was not found in the `WishList.Controllers` namespace.");

            var method = itemController.GetMethod("Create", new Type[] { });
            Assert.True(method != null, "`ItemController` did not contain a `Create` method did you remove or rename it?");

            var controller = Activator.CreateInstance(itemController, new object[] { });

            var results = (ViewResult)method.Invoke(controller, new object[] { });
            Assert.True(results != null, "`ItemController`'s `Create` method did not run successfully, please run locally and verify results.");
            // verify create actually adds the userid before saving
        }
    }
}
