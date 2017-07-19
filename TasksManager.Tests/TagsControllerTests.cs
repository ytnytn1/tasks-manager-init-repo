using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public class TagsControllerTests
    {
        [Fact]
        public async void GetTags_Produces_200_Status()
        {
            //arrange
            var controller = TestHelper.CreateController<TagsController>();
            var mockQuery = new Mock<ITagsListQuery>(MockBehavior.Strict);
            var filter = new TagsFilter();
            var options = new ListOptions();
            var tagsList = new ListResponse<TagsResponse>();
            mockQuery.Setup(op => op.RunAsync(It.IsAny<TagsFilter>(), It.IsAny<ListOptions>()))
                .ReturnsAsync(tagsList);
            //act
           
            var result = await  controller.GetTagsListAsync(filter, options, mockQuery.Object);
            var objectResult = result as OkObjectResult;
            //assert
            mockQuery.Verify(op => op.RunAsync(It.IsAny<TagsFilter>(), It.IsAny<ListOptions>()));
            TestHelper.AssertObjectResult(200, objectResult);
        }

        [Fact]
        public async void DeleteTag_Produces_204()
        {
            //arrange
            var controller = TestHelper.CreateController<TagsController>();
            var moqCmd = new Mock<IDeleteTagCommand>();
            moqCmd.Setup(op => op.ExecuteAsync(0)).Returns(Task.CompletedTask);

            //act
            var actionResult = await ActDeleteOp(controller, moqCmd);

            //assert
            moqCmd.Verify(op => op.ExecuteAsync(0));
            TestHelper.AsserStatusCodeResult(204, actionResult);

        }

        [Fact]
        public async void DeleteTag_Produces_500()
        {
            //arrange
            var controller = TestHelper.CreateController<TagsController>();
            var moqCmd = new Mock<IDeleteTagCommand>();
            moqCmd.Setup(op => op.ExecuteAsync(0)).Throws<Exception>();

            //act
            var actionResult = await ActDeleteOp(controller, moqCmd);

            //assert
            moqCmd.Verify(op => op.ExecuteAsync(0));
            TestHelper.AsserStatusCodeResult(500, actionResult);
        }

        private async Task<StatusCodeResult> ActDeleteOp(TagsController controller, Mock<IDeleteTagCommand> moqOp)
        {
            var result = await controller.DeleteTagAsync(0, moqOp.Object);
            var actionResult = result as StatusCodeResult;
            return actionResult;
        }
        
    }
}
