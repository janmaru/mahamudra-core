using Mahamudra.Core.Errors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTestsCore
{
    [TestClass]
    public class ErrorTest
    {
        [TestMethod]
        public void Error_Constructor_ShouldSetProperties()
        {
            var error = new Error(400, "Bad Request", "Invalid input");

            Assert.AreEqual(400, error.Code);
            Assert.AreEqual("Bad Request", error.Description);
            Assert.AreEqual("Invalid input", error.Message);
        }

        [TestMethod]
        public void Error_Validation_ShouldCreate400Error()
        {
            var error = Error.Validation("Invalid email");

            Assert.AreEqual(400, error.Code);
            Assert.AreEqual("Invalid email", error.Description);
        }

        [TestMethod]
        public void Error_NotFound_ShouldCreate404Error()
        {
            var error = Error.NotFound("User not found");

            Assert.AreEqual(404, error.Code);
            Assert.AreEqual("User not found", error.Description);
        }

        [TestMethod]
        public void Error_Conflict_ShouldCreate409Error()
        {
            var error = Error.Conflict("Email already exists");

            Assert.AreEqual(409, error.Code);
            Assert.AreEqual("Email already exists", error.Description);
        }

        [TestMethod]
        public void Error_Unauthorized_ShouldCreate401Error()
        {
            var error = Error.Unauthorized("Invalid credentials");

            Assert.AreEqual(401, error.Code);
            Assert.AreEqual("Invalid credentials", error.Description);
        }

        [TestMethod]
        public void Error_Forbidden_ShouldCreate403Error()
        {
            var error = Error.Forbidden("Access denied");

            Assert.AreEqual(403, error.Code);
            Assert.AreEqual("Access denied", error.Description);
        }

        [TestMethod]
        public void Error_Internal_ShouldCreate500Error()
        {
            var error = Error.Internal("Database connection failed");

            Assert.AreEqual(500, error.Code);
            Assert.AreEqual("Database connection failed", error.Description);
        }

        [TestMethod]
        public void Error_BadRequest_ShouldCreate400Error()
        {
            var error = Error.BadRequest("Missing required field");

            Assert.AreEqual(400, error.Code);
            Assert.AreEqual("Missing required field", error.Description);
        }

        [TestMethod]
        public void Error_IsRecord_ShouldSupportValueEquality()
        {
            var error1 = new Error(400, "Bad Request", "Invalid input");
            var error2 = new Error(400, "Bad Request", "Invalid input");

            Assert.AreEqual(error1, error2);
        }

        [TestMethod]
        public void Error_DifferentValues_ShouldNotBeEqual()
        {
            var error1 = new Error(400, "Bad Request", "Invalid input");
            var error2 = new Error(404, "Not Found", "Resource not found");

            Assert.AreNotEqual(error1, error2);
        }
    }

    [TestClass]
    public class ValidationErrorTest
    {
        [TestMethod]
        public void ValidationError_Constructor_ShouldSetProperties()
        {
            var fieldErrors = new Dictionary<string, string[]>
            {
                ["Email"] = new[] { "Email is required", "Invalid format" },
                ["Name"] = new[] { "Name is required" }
            };

            var error = new ValidationError("Validation failed", "Please fix errors", fieldErrors);

            Assert.AreEqual(400, error.Code);
            Assert.AreEqual("Validation failed", error.Description);
            Assert.AreEqual("Please fix errors", error.Message);
            Assert.IsTrue(error.HasFieldErrors);
            Assert.HasCount(2, error.FieldErrors);
        }

        [TestMethod]
        public void ValidationError_ForField_ShouldCreateSingleFieldError()
        {
            var error = ValidationError.ForField("Email", "Email is required", "Invalid format");

            Assert.AreEqual(400, error.Code);
            Assert.IsTrue(error.HasFieldErrors);
            Assert.HasCount(1, error.FieldErrors);
            Assert.IsTrue(error.FieldErrors.ContainsKey("Email"));
            Assert.HasCount(2, error.FieldErrors["Email"]);
        }

        [TestMethod]
        public void ValidationError_ForFields_ShouldCreateMultipleFieldErrors()
        {
            var fieldErrors = new Dictionary<string, string[]>
            {
                ["Email"] = new[] { "Email is required" },
                ["Password"] = new[] { "Password too weak" }
            };

            var error = ValidationError.ForFields(fieldErrors, "Invalid input");

            Assert.AreEqual(400, error.Code);
            Assert.AreEqual("Invalid input", error.Description);
            Assert.IsTrue(error.HasFieldErrors);
            Assert.HasCount(2, error.FieldErrors);
        }

        [TestMethod]
        public void ValidationError_WithoutFieldErrors_ShouldHaveEmptyDictionary()
        {
            var error = new ValidationError("General validation error");

            Assert.AreEqual(400, error.Code);
            Assert.IsFalse(error.HasFieldErrors);
            Assert.IsEmpty(error.FieldErrors);
        }

        [TestMethod]
        public void ValidationError_IsRecord_ShouldSupportValueEquality()
        {
            var fieldErrors = new Dictionary<string, string[]>
            {
                ["Email"] = new[] { "Required" }
            };

            var error1 = new ValidationError("Validation failed", string.Empty, fieldErrors);
            var error2 = new ValidationError("Validation failed", string.Empty, fieldErrors);

            Assert.AreEqual(error1, error2);
        }

        [TestMethod]
        public void ValidationError_InheritsFromError()
        {
            var error = ValidationError.ForField("Email", "Required");

            Assert.IsInstanceOfType(error, typeof(Error));
        }
    }
}
