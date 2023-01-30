using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDo.Models
{
    public class Project
    {
        [Required]
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; } = string.Empty;
        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        [Display(Name = "Visibility")]
        public bool isHidden { get; set; } = false;
        [Display(Name = "Type Of Project")]
        public Enums.TypeOfProject Type { get; set; } = Enums.TypeOfProject.Console;
        [Display(Name = "Description")]
        public string? Description { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Images Path")]
        public string ImagesPath { get; set; } = string.Empty;
        [NotMapped]
        [Display(Name = "Images")]
        public List<string>? Images { get; set; } = null;
        [Display(Name = "Tasks")]
        public List<Task>? Tasks { get; set; } = null;

    }
}