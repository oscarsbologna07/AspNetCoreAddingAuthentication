using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace WishListTests
{
    public class UpdateItemModelTests
    {
        [Fact(DisplayName = "Update Item Model @update-item-model")]
        public void UpdateItemModel()
        {
            var filePath = ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "WishList" + Path.DirectorySeparatorChar + "Models" + Path.DirectorySeparatorChar + "Item.cs";
            Assert.True(File.Exists(filePath), @"`Item.cs` was not found in the `Models` folder, did you accidentally rename or remove it?");

            var item = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                  from type in assembly.GetTypes()
                                  where type.FullName == "WishList.Models.Item"
                                  select type).FirstOrDefault();
            Assert.True(item != null, "A `public` class `Item` was not found in the `WishList.Models` namespace.");

            var userIdProperty = item.GetProperty("UserId");
            Assert.True(userIdProperty != null, "`Item` does not appear to contain a `public` `string` property `UserId`.");
            Assert.True(userIdProperty.PropertyType == typeof(string), "`Item` has a property `UserId` but it is not of type `string`.");
        }
    }
}
