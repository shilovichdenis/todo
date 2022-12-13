namespace ToDo.Models.ViewComponents
{
    public class ProjectViewComponent
    {
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool isHidden { get; set; }
        public Enums.TypeOfProject Type { get; set; }
        public string? Description { get; set; }
        public string? ImagesPath { get; set; }
        public List<IFormFile>? Images { get; set; }
    }
}
