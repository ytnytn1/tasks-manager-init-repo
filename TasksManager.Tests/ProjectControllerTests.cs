using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TasksManager.Controllers;
using TasksManager.DataAccess.DbImplementation.Projects;
using TasksManager.DataAccess.Projects;
using TasksManager.DataAccess.Tags;
using TasksManager.ViewModel;
using TasksManager.ViewModel.Projects;
using Xunit;

namespace TasksManager.Tests
{
    
    public class ProjectControllerTests
    {
        [Fact]
        public async void GetProjects_Produces_200_Status()
        {
            //arrange
            var controller = TestHelper.CreateController<ProjectsController>();
            var mockQuery = new Mock<IProjectsListQuery>(MockBehavior.Strict);
            var filter = new ProjectFilter();
            var options = new ListOptions();
            var tagsList = TestHelper.CreateResponse<ProjectResponse>();
            mockQuery.Setup(op => op.RunAsync(It.IsAny<ProjectFilter>(), It.IsAny<ListOptions>()))
                .ReturnsAsync(tagsList);
            //act

            var result = await controller.GetProjectsListAsync(filter, options, mockQuery.Object);
            var objectResult = result as OkObjectResult;
            //assert
            mockQuery.Verify(op => op.RunAsync(It.IsAny<ProjectFilter>(), It.IsAny<ListOptions>()));
            TestHelper.AssertObjectResult(200, objectResult);
        }

        [Fact]
        public async void CreateProject_Returns_CreatedAtAction_GetProjectAsync()
        {
            //arrange
            var controller = TestHelper.CreateController<ProjectsController>();
            var request = new CreateProjectRequest {Description = "1", Name = "1"};
            var mockCmd = new Mock<ICreateProjectCommand>();
            mockCmd.Setup(cmd => cmd.ExecuteAsync(It.IsAny<CreateProjectRequest>()))
                .ReturnsAsync(new ProjectResponse {Id = 1});

            //act
            var result = await controller.CreateProjectAsync(request, mockCmd.Object);
            //assert
            mockCmd.Verify(cmd => cmd.ExecuteAsync(It.IsAny<CreateProjectRequest>()));
            TestHelper.AssertCreatedAtActionResult(result as CreatedAtActionResult, "GetProjectAsync");
        }

        [Fact]
        public async void CreateProject_Returns_BadRequest_If_Model_State_IsNotValid()
        {
            var controller = TestHelper.CreateController<ProjectsController>();
            var request = new CreateProjectRequest { Description = "1", Name = "1" };
            var mockCmd = new Mock<ICreateProjectCommand>();
            mockCmd.Setup(cmd => cmd.ExecuteAsync(It.IsAny<CreateProjectRequest>()))
                .ReturnsAsync(new ProjectResponse { Id = 1 });
            controller.ModelState.AddModelError("","");
            //act
            var result = await controller.CreateProjectAsync(request, mockCmd.Object);
            //assert
            mockCmd.Verify(c => c.ExecuteAsync(It.IsAny<CreateProjectRequest>()), Times.Never);
            TestHelper.AssertBadRequestObjectResult(result as BadRequestObjectResult);
        }

        [Fact]
        public async void GetProject_Returns_Ok_ObjectResult()
        {
            //arrange
            var controller = TestHelper.CreateController<ProjectsController>();
            var mockQuery = new Mock<IProjectQuery>();
            mockQuery.Setup(q => q.RunAsync(It.IsAny<int>())).ReturnsAsync(new ProjectResponse
            {
                Id = 1
            });
            //act

            var reuslt = await controller.GetProjectAsync(1, mockQuery.Object);
            
            //assert
            mockQuery.Verify(q => q.RunAsync(It.IsAny<int>()));
            TestHelper.AssertObjectResult(200, reuslt as ObjectResult);
        }

        [Fact]
        public async void GetProject_Returns_NotFound()
        {
            //arrange
            var controller = TestHelper.CreateController<ProjectsController>();
            var mockQuery = new Mock<IProjectQuery>();
            mockQuery.Setup(q => q.RunAsync(It.IsAny<int>())).ReturnsAsync((ProjectResponse) null);
            //act

            var result = await controller.GetProjectAsync(1, mockQuery.Object);

            //assert
            mockQuery.Verify(q => q.RunAsync(It.IsAny<int>()));
            TestHelper.AssertNotFoundResult(result as NotFoundResult);
        }

        [Fact]
        public async void UpdateProject_Returns_Ok_ObjectResult()
        {
            //arrange
            var controller = TestHelper.CreateController<ProjectsController>();
            var mockCmd = new Mock<IUpdateProjectCommand>();
            mockCmd.Setup(q => q.ExecuteAsunc(It.IsAny<int>(), It.IsAny<UpdateProjectRequest>())).ReturnsAsync(new ProjectResponse
            {
                Id = 1
            });
            //act

            var result = await controller.UpdateProjectAsync(1, new UpdateProjectRequest(), mockCmd.Object);

            //assert
            mockCmd.Verify(q => q.ExecuteAsunc(It.IsAny<int>(), It.IsAny<UpdateProjectRequest>()));
            TestHelper.AssertObjectResult(200, result as ObjectResult);
        }

        [Fact]
        public async void UpdateProject_Returns_Ok_NotFoundResult()
        {
            //arrange
            var controller = TestHelper.CreateController<ProjectsController>();
            var mockCmd = new Mock<IUpdateProjectCommand>();
            mockCmd.Setup(q => q.ExecuteAsunc(It.IsAny<int>(), It.IsAny<UpdateProjectRequest>()))
                .ReturnsAsync((ProjectResponse) null);
            //act

            var result = await controller.UpdateProjectAsync(1, new UpdateProjectRequest(), mockCmd.Object);

            //assert
            mockCmd.Verify(q => q.ExecuteAsunc(It.IsAny<int>(), It.IsAny<UpdateProjectRequest>()));
            TestHelper.AssertNotFoundResult(result as NotFoundResult);
        }

        [Fact]
        public async void UpdateProject_Returns_BadRequest()
        {
            //arrange
            var controller = TestHelper.CreateController<ProjectsController>();
            var mockCmd = new Mock<IUpdateProjectCommand>();
            mockCmd.Setup(q => q.ExecuteAsunc(It.IsAny<int>(), It.IsAny<UpdateProjectRequest>()))
                .ReturnsAsync((ProjectResponse)null);
            controller.ModelState.AddModelError("","");
            //act
            
            var result = await controller.UpdateProjectAsync(1, new UpdateProjectRequest(), mockCmd.Object);

            //assert
            mockCmd.Verify(q => q.ExecuteAsunc(It.IsAny<int>(), It.IsAny<UpdateProjectRequest>()), Times.Never);
            TestHelper.AssertBadRequestObjectResult(result as BadRequestObjectResult);
        }

        //[Fact]
        //public async void DeleteTag_Produces_204()
        //{
        //    //arrange
        //    var controller = TestHelper.CreateController<TagsController>();
        //    var moqCmd = new Mock<IDeleteTagCommand>();
        //    moqCmd.Setup(op => op.ExecuteAsync(0)).Returns(Task.CompletedTask);

        //    //act
        //    var actionResult = await ActDeleteOp(controller, moqCmd);

        //    //assert
        //    moqCmd.Verify(op => op.ExecuteAsync(0));
        //    TestHelper.AsserStatusCodeResult(204, actionResult);

        //}

        [Fact]
        public async void DeleteProject_Retuns_InternalServerError()
        {
            //arrange
            var controller = TestHelper.CreateController<ProjectsController>();
            var moqCmd = new Mock<IDeleteProjectCommand>();
            moqCmd.Setup(op => op.ExecuteAsync(It.IsAny<int>())).Throws<Exception>();

            //act
            var result = await controller.DeleteProjectAsync(1, moqCmd.Object);

            //assert
            moqCmd.Verify(op => op.ExecuteAsync(It.IsAny<int>()));
            TestHelper.AsserStatusCodeResult(500, (StatusCodeResult) result);
        }

        [Fact]
        public async void DeleteProject_Returns_BadRequestResult()
        {
            //arrange
            var controller = TestHelper.CreateController<ProjectsController>();
            var moqCmd = new Mock<IDeleteProjectCommand>();
            moqCmd.Setup(op => op.ExecuteAsync(It.IsAny<int>())).Throws<CannotDeleteProjectWithTasksException>();

            //act

            var result = await controller.DeleteProjectAsync(1, moqCmd.Object);

            //assert
            moqCmd.Verify(op => op.ExecuteAsync(It.IsAny<int>()));
            TestHelper.AssertBadRequestObjectResult(result as BadRequestObjectResult);
        }

        

    }
}
