using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OneTestApi.Models
{
    public class TestProject
    {
        public TestProject()
        {
            this.TestSuites = new List<TestSuite>();
        }

        public int Id { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public List<TestSuite> TestSuites { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime InsertTime { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdateTime { get; set; }
    }
}