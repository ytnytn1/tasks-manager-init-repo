using Microsoft.AspNetCore.Mvc;
using TasksManager.ViewModel;
using Xunit;

namespace TasksManager.Tests
{
    internal static class TestHelper
    {
        internal static TController CreateController<TController>() where TController : ControllerBase ,new()
        {
            return new TController();
        }

        internal static ListResponse<TResponse> CreateResponse<TResponse>() where TResponse : class
        {
            return new ListResponse<TResponse>();
        }

        internal static void AssertObjectResult(int expected, ObjectResult objectResult)
        {
            Assert.NotNull(objectResult);
            Assert.NotNull(objectResult.Value);
            Assert.Equal(expected, objectResult.StatusCode);
        }

        internal static void AsserStatusCodeResult(int expected, StatusCodeResult actionResult)
        {
            Assert.NotNull(actionResult);
            Assert.Equal(expected, actionResult.StatusCode);
        }

        internal static void AssertCreatedAtActionResult(CreatedAtActionResult createdAtActionResult, string expectedActionName)
        {
            Assert.NotNull(createdAtActionResult);
            Assert.NotNull(createdAtActionResult.Value);
            Assert.Equal(expectedActionName, createdAtActionResult.ActionName);
        }

        internal static void AssertBadRequestObjectResult(BadRequestObjectResult badRequestResult)
        {
            Assert.NotNull(badRequestResult);
            Assert.NotNull(badRequestResult.Value);            
        }

        internal static void AssertBadRequestResult(BadRequestResult badRequestResult)
        {
            Assert.NotNull(badRequestResult);;
        }

        internal static void AssertNotFoundResult(NotFoundResult notFoundResult)
        {
            Assert.NotNull(notFoundResult); ;
        }
    }
}
