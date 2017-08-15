using System;
using System.Collections.Generic;

namespace OneTestApi.Controllers.DTOs
{
    public class TestSuiteDto: TestNodeDto
    {
        public string Description { get; set; }
        
        public DateTime InsertTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}