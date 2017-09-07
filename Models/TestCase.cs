using System;
using System.Collections.Generic;

namespace OneTestApi.Models
{
    public enum TestCaseExecutionType
    {
        Manual,
        Automated
    }

    public enum TestCaseImportance
    {
        High,
        Medium,
        Low
    }
    
    public class TestCase : TestNode
    {
        public string Summary { get; set; }
        
        public string Precondition { get; set; }

        public TestCaseExecutionType ExecutionType { get; set; }
        
        public TestCaseImportance Importance { get; set; }
        
        public List<TestCaseTag> Tags { get; set; } = new List<TestCaseTag>();

        public List<TestStep> TestSteps { get; set; } = new List<TestStep>();
    }
}