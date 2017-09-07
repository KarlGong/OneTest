using System;
using System.Collections.Generic;

namespace OneTestApi.Models
{
    public abstract class TestNode
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public int Position { get; set; }
        
        public int? ParentId { get; set; }
        
        public TestNode Parent { get; set; }

        public List<TestNode> Children { get; set; } = new List<TestNode>();

        public int Count { get; set; }
        
        public DateTime InsertTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public bool IsDeleted { get; set; }
    }
}