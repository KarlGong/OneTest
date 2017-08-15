using System;
using System.Collections.Generic;
using OneTestApi.Models;

namespace OneTestApi.Controllers.DTOs
{
    public class TestCaseDto: TestNodeDto
    {
        public string Summary { get; set; }
        
        public string Precondition { get; set; }

        public TestCaseExecutionType ExecutionType { get; set; }
        
        public TestCaseImportance Importance { get; set; }
        
        public List<TestCaseTagDto> Tags { get; set; } = new List<TestCaseTagDto>();

        public List<TestStepDto> TestSteps { get; set; } = new List<TestStepDto>();
        
        public DateTime InsertTime { get; set; }
        
        public DateTime UpdateTime { get; set; }
    }
}