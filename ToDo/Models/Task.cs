namespace ToDo.Models
{
    public class Task
    {
        public int Id { get; set; }
        public Project? Project { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string CreatedDate { get; set; }
        public string? CompletedDate { get; set; }
        public bool isCompleted { get; set; }
        public Priority Priority { get; set; }
    }

    public enum Priority
    {
        Low,
        Medium,
        High,
        Critical
    }
}
