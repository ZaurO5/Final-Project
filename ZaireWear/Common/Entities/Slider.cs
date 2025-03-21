namespace Core.Entities;
    public class Slider : BaseEntity
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string ImagePath { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
    }
