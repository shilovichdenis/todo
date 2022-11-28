using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ToDo.Models
{
    public class Task
    {
        public int Id { get; set; }
        [BindNever]
        public Project? Project { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public bool isCompleted { get; set; }
        public Priority Priority { get; set; }
        public string? Description { get; set; }
    }

    public enum Priority
    {
        Low,
        Medium,
        High,
        Critical
    }
}
