using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TasksManager.DataAccess.Projects;
using TasksManager.ViewModel;
using TasksManager.ViewModel.Projects;

namespace TasksManager.Controllers
{
    [Route("api/[controller]")]
    public class ProjectsController : Controller
    {
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ListResponse<ProjectResponse>))]
        public async Task<IActionResult> GetProjectsListAsync(ProjectFilter filter, ListOptions options, [FromServices]IProjectsListQuery query)
        {
            var response = await query.RunAsync(filter, options);
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ProjectResponse))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateProjectAsync([FromBody]CreateProjectRequest request, [FromServices]ICreateProjectCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ProjectResponse response = await command.ExecuteAsync(request);
            return CreatedAtAction("GetProjectAsync", new { projectId = response.Id }, response);
        }

        [HttpGet("{projectId}")]
        [ProducesResponseType(200, Type = typeof(ProjectResponse))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProjectAsync(int projectId, [FromServices]IProjectQuery query)
        {
           ProjectResponse response = await query.RunAsync(projectId);
           return response == null
                ? (IActionResult)NotFound()
                : Ok(response);
        }

        [HttpPut("{projectId}")]
        [ProducesResponseType(200, Type = typeof(ProjectResponse))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateProjectAsync(int projectId, [FromBody]UpdateProjectRequest request, [FromServices] IUpdateProjectCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await command.ExecuteAsunc(projectId, request);
            return response == null
                ? (IActionResult)NotFound()
                : Ok(response);
        }

        [HttpDelete("{projectId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteProjectAsync(int projectId, [FromServices] IDeleteProjectCommand command)
        {
            try
            {
                await command.ExecuteAsync(projectId);
                return new StatusCodeResult(204);
            }
            catch (CannotDeleteProjectWithTasksException e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new StatusCodeResult(500);
            }
        }
    }
}
