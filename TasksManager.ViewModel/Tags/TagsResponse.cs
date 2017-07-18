using System;
using System.Collections.Generic;
using System.Text;

namespace TasksManager.ViewModel.Tags
{
    public class TagsResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int TasksCount { get; set; }

        public int OpenTasksCount { get; set; }
    }
}
