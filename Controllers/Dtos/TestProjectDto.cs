using System;
using System.Collections.Generic;

namespace OneTestApi.Controllers.DTOs
{
    public class TestProjectDto: TestNodeDto
    {
        public string Description { get; set; }
        
        public DateTime InsertTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}