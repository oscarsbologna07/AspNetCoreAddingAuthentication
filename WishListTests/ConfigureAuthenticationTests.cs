using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Xunit;

namespace WishListTests
{
    public class ConfigureAuthenticationTests
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

        [Fact(DisplayName = "Update ApplicationDbContexts Inherittance @update-applicationdbcontexts-inherittance")]
        public void UpdateApplicationDbContextInherritenceTest()
        {
            var applicationDbContext = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                        from type in assembly.GetTypes()
                                        where type.FullName == "WishList.Data.ApplicationDbContext"
                                        select type).FirstOrDefault();
            
            Assert.True(applicationDbContext != null, "A `public` class `ApplicationDbContext` was not found in the `WishList.Data` namespace, was it accidentally renamed or deleted?");
            Assert.True(applicationDbContext.BaseType == typeof(IdentityDbContext), "`ApplicationDbContext` is not inheritting the `IdentityDbContext` class.");
            // #todo verify IdentityDbContext<ApplicationUser> not just IdentityDbContext (this will probably require regex)
        }

        [Fact(DisplayName = "Call AddIdentity In Configure @call-addidentity-in-configureservices")]
        public void CallAddIdentityInConfigureServicesTest()
        {
            var filePath = ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "WishList" + Path.DirectorySeparatorChar + "StartUp.cs";
            Assert.True(File.Exists(filePath), "`StartUp.cs` was not found. Did you rename or delete it?");

            string file;
            using (var streamReader = new StreamReader(filePath))
            {
                file = streamReader.ReadToEnd();
            }

            var pattern = @"services/s*?[.]AddIdentity/s*?[<]/s*?ApplicationUser/s*?,/s*?IdentityRole/s*?[>]/s*?[(]/s*?[)]/s*?[.]AddEntityFrameworkStores/s*?[<]/s*?ApplicationDbContext/s*?[>]/s*?[(]/s*?[)]/s*?[.]AddDefaultTokenProviders/s*?[(]/s*?[)]/s*?;";
            var rgx = new Regex(pattern);

            Assert.True(rgx.IsMatch(file), "`StartUp.ConfigureServices` didn't contain a call the `AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();` on `services`.");
        }

        [Fact(DisplayName = "Call UseAuthentication In ConfigureServices @call-useauthentication-in-configure")]
        public void CallUseAuthenticationInConfigureTest()
        {
            var filePath = ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "WishList" + Path.DirectorySeparatorChar + "StartUp.cs";
            Assert.True(File.Exists(filePath), "`StartUp.cs` was not found. Did you rename or delete it?");

            string file;
            using (var streamReader = new StreamReader(filePath))
            {
                file = streamReader.ReadToEnd();
            }

            var pattern = @"app.UseAuthentication[(]\s*?[)]\s*?;\s*?app.UseMvc";
            var rgx = new Regex(pattern);

            Assert.True(rgx.IsMatch(file), "`StartUp.Configure` should call `UseAuthentication` before `UseMvcWithDefaultRoute`.");
        }
    }
}
