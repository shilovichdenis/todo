using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ToDo.Models.View
{
    public class DisplayView
    {
        public Project Project { get; set; }
        [BindNever]
        public List<Task>? Tasks { get; set; }
        [BindNever]
        public string? StatusMessage { get; set; }

    }
}
