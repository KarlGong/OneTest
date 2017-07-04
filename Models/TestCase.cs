using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OneTestApi.Models
{
    public class TestCase
    {
        public TestCase()
        {
            this.TestSteps = new List<TestStep>();
        }

        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Summary { get; set; }
        
        public string Precondition { get; set; }
        
        public int Order { get; set; }

        public TestSuite TestSuite { get; set; }
        
        public List<TestStep> TestSteps { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime InsertTime { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdateTime { get; set; }
    }
}