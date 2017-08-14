using System;

namespace OneTestApi.Controllers.DTOs
{
    public class TestProjectDto
    {
        public int Id { get; set; }
        
        public int Position { get; set; }
        
        public string Name { get; set; }
        
        public string Type = "project";
        
        public string Description { get; set; }
        
        public DateTime InsertTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}