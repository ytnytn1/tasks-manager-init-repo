using System.Threading.Tasks;
using TasksManager.ViewModel;
using TasksManager.ViewModel.Tags;

namespace TasksManager.DataAccess.Tags
{
    public interface ITagsListQuery
    {
        Task<ListResponse<TagsResponse>> RunAsync(TagsFilter filter, ListOptions listOptions);
    }
}
