using System;

namespace OneTestApi.Controllers.DTOs
{
    public class TestSuiteDto
    {
        public int Id { get; set; }
        
        public int Position { get; set; }
        
        public string Name { get; set; }
        
        public string Type = "suite";
        
        public string Description { get; set; }
        
        public DateTime InsertTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}