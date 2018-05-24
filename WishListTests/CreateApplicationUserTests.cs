using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace WishListTests
{
    public class CreateApplicationUserTests
    {
        [Fact(DisplayName = "Create ApplicationUser Model @create-applicationuser-model")]
        public void CreateApplicationUserTest()
        {
            var filePath = ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "WishList" + Path.DirectorySeparatorChar + "Models" + Path.DirectorySeparatorChar + "ApplicationUser.cs";
            Assert.True(File.Exists(filePath), "`ApplicationUser.cs` was not found in the `Models` folder.");

            var applicationUserModel = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                        from type in assembly.GetTypes()
                                        where type.FullName == "WishList.Models.ApplicationUser"
                                        select type).FirstOrDefault();

            Assert.True(applicationUserModel != null, "`ApplicationUser.cs` was found but doesn't appear to contain a `public` class `ApplicationUser` in the `WishList.Models` namespace.");
            Assert.True(applicationUserModel.BaseType == typeof(IdentityUser), "`ApplicationUser` was found but does not appear to inherit the `IdentityUser` class.");
        }
    }
}
