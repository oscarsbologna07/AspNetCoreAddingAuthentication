using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
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
    }
}
