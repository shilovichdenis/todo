using System.ComponentModel.DataAnnotations.Schema;

namespace ToDo.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedDate { get; set; }
        public ProgrammingLanguage ProgrammingLanguage { get; set; }
        public Type Type { get; set; }
        public List<Task>? Tasks { get; set; }

    }
    public enum ProgrammingLanguage
    {
        CPlusPlus = 0,
        CSharp = 1,
        Java = 2,
        Python = 3,
        HTML = 4,
        C1 = 5
    }
    public enum Type
    {
        Web = 0,
        Console = 1,
        Desktop = 2,
        Mobile = 3
    }

}
