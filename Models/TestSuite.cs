﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OneTestApi.Models
{
    public class TestSuite
    {
        public TestSuite()
        {
            this.TestSuites = new List<TestSuite>();
            this.TestCases = new List<TestCase>();
        }

        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }

        public List<TestSuite> TestSuites { get; set; }

        public List<TestCase> TestCases { get; set; }
        
        public int Order { get; set; }
        
        public int Count { get; set; }
        
        public TestSuite ParentTestSuite { get; set; }
        
        public TestProject TestProject { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime InsertTime { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdateTime { get; set; }
    }
}
