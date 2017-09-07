using System;

namespace OneTestApi.Models
{
    public class TestCaseTag
    {
        public int Id { get; set; }
        
        public string Value { get; set; }
        
        public int TestCaseId { get; set; }
        
        public TestCase TestCase { get; set; }
        
        public DateTime InsertTime { get; set; }
        
        public DateTime UpdateTime { get; set; }
    }
}