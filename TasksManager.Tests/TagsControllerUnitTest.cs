using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TasksManager.Controllers;
using TasksManager.DataAccess.Tags;
using TasksManager.ViewModel;
using TasksManager.ViewModel.Tags;
using Xunit;

namespace TasksManager.Tests
{
    public class TagsControllerUnitTest
    {
        [Fact]
        public async void GetTags_Produces_200_Status()
        {
            //arrange
            var controller = CreateTagController();
            var moqOp = new Mock<ITagsListQuery>();
            var filter = new TagsFilter();
            var options = new ListOptions();
            moqOp.Setup(op => op.RunAsync(new TagsFilter(), new ListOptions()))
                .ReturnsAsync(new ListResponse<TagsResponse>
                {
                    Items = new List<TagsResponse>
                    {
                        new TagsResponse {Id = 1, Name = "Tag1", OpenTasksCount = 1, TasksCount = 1},
                        new TagsResponse {Id = 2, Name = "Tag2", OpenTasksCount = 1, TasksCount = 1}
                    }
                });
            //act
            var result = await  controller.GetTagsistAsync(filter, options, moqOp.Object);
            var actionResult = result as OkObjectResult;
            //assert
            moqOp.Verify(op => op.RunAsync(filter, options));
            AsserActionResult(200, new StatusCodeResult((int) actionResult.StatusCode));
        }

        [Fact]
        public async void DeleteTag_Produces_204()
        {
            //arrange
            var controller = CreateTagController();
            var moqOp = new Mock<IDeleteTagCommand>();
            moqOp.Setup(op => op.ExecuteAsync(0)).Returns(Task.CompletedTask);

            //act
            var actionResult = await ActDeleteOp(controller, moqOp);

            //assert
            moqOp.Verify(op => op.ExecuteAsync(0));
            AsserActionResult(204,actionResult);

        }

        private TagsController CreateTagController()
        {
            var controller = new TagsController();
            return controller;
        }

        [Fact]
        public async void DeleteTag_Produces_500()
        {
            //arrange
            var controller = CreateTagController();
            Mock<IDeleteTagCommand> moqOp = new Mock<IDeleteTagCommand>();
            moqOp.Setup(op => op.ExecuteAsync(0)).Throws<Exception>();

            //act
            var actionResult = await ActDeleteOp(controller, moqOp);

            //assert
            moqOp.Verify(op => op.ExecuteAsync(0));
            AsserActionResult(500, actionResult);
        }

        private async Task<StatusCodeResult> ActDeleteOp(TagsController controller, Mock<IDeleteTagCommand> moqOp)
        {
            var result = await controller.DeleteTagAsync(0, moqOp.Object);
            var actionResult = result as StatusCodeResult;
            return actionResult;
        }

        private void AsserActionResult(int expected, StatusCodeResult actionResult)
        {
            Assert.NotNull(actionResult);
            Assert.Equal(expected, actionResult.StatusCode);
        }
    }
}
