using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDo.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool isHidden { get; set; }
        public Enums.TypeOfProject Type { get; set; }
        public string? Description { get; set; }
        public string? ImagesPath { get; set; }
        [NotMapped]
        public List<string>? Images { get; set; }
        public List<Task>? Tasks { get; set; }

    }
}