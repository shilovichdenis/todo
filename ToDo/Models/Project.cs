using System.ComponentModel.DataAnnotations.Schema;

namespace ToDo.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool isHidden { get; set; }
        public Type Type { get; set; }
        public string? Description { get; set; }
        public List<Task>? Tasks { get; set; }

    }
    public enum Type
    {
        Website,
        Console,
        Desktop,
        Game,
        Mobile,
        Video,
        Music,
        Doc,
        Table,
        Design
    }

}
