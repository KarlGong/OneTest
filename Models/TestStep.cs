using System;

namespace OneTestApi.Models
{
    public class TestStep
    {
        public int Id { get; set; }
        
        public string Action { get; set; }
        
        public string ExpectedResult { get; set; }
        
        public int TestCaseId { get; set; }
        
        public TestCase TestCase { get; set; }
        
        public DateTime InsertTime { get; set; }
        
        public DateTime UpdateTime { get; set; }
    }
}