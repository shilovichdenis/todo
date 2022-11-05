using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ToDo.Models
{
    public class Machine
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        [Display(Name = "Лента")]
        public string? ConveyorBelt { get; set; }
        public string? Belt { get; set; }
        public string? Bearing { get; set; }
        public string? ImagesPath { get; set; }
    }
}
