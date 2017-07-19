using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TasksManager.DataAccess.Tags;
using TasksManager.ViewModel;
using TasksManager.ViewModel.Tags;

namespace TasksManager.Controllers
{
    [Route("api/[controller]")]
    public class TagsController:Controller
    {
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ListResponse<TagsResponse>))]
        public async Task<IActionResult> GetTagsListAsync(TagsFilter filter, ListOptions options, [FromServices]ITagsListQuery query)
        {
            var response = await query.RunAsync(filter, options);
            return Ok(response);
        }

        [HttpDelete("{tagId}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteTagAsync(int tagId, [FromServices] IDeleteTagCommand deleteTagCommand)
        {
            try
            {
                await deleteTagCommand.ExecuteAsync(tagId);
                return new StatusCodeResult(204);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new StatusCodeResult(500);
            }
        }
    }
}
