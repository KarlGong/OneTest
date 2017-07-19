using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OneTestApi.Models
{
    public enum ExecutionType
    {
        Manual,
        Automated
    }

    public enum Importance
    {
        High,
        Medium,
        Low
    }
    
    public class TestCase
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Summary { get; set; }
        
        public string Precondition { get; set; }

        public ExecutionType ExecutionType { get; set; }
        
        public Importance Importance { get; set; }
        
        public List<TestCaseTag> Tags { get; set; } = new List<TestCaseTag>();

        public int Order { get; set; }

        public TestSuite TestSuite { get; set; }
        
        public List<TestStep> TestSteps { get; set; } = new List<TestStep>();
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime InsertTime { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdateTime { get; set; }
    }
}