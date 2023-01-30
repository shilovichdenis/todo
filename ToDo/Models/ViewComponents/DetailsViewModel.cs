using Microsoft.Build.Framework;

namespace ToDo.Models.ViewComponents
{
    public class DetailsViewModel
    {
        public Project Project { get; set; }
        public string? StatusMessage { get; set; }
    }
}
