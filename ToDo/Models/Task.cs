using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ToDo.Models
{
    public class Task
    {
        [Required]
        [Display(Name = "Id")]
        public int Id { get; set; }
        [BindNever]
        [Display(Name = "Project")]
        public Project? Project { get; set; } = null;
        [Required]
        [Display(Name = "Project Id")]
        public int ProjectId { get; set; } = 0;
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; } = string.Empty;
        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        [Display(Name = "Completed Date")]
        public DateTime? CompletedDate { get; set; }
        [Display(Name = "Execution Status")]
        public bool isCompleted { get; set; } = false;
        [Display(Name = "Priority")]
        public Enums.Priority Priority { get; set; } = Enums.Priority.Low;
        [Display(Name = "Description")]
        public string? Description { get; set; } = string.Empty;
    }
}
