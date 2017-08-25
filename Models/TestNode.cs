using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OneTestApi.Models
{
    public abstract class TestNode
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public int Position { get; set; }
        
        public int? ParentId { get; set; }
        
        [ForeignKey("ParentId")]
        public TestNode Parent { get; set; }

        public List<TestNode> Children { get; set; } = new List<TestNode>();

        public int Count { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime InsertTime { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdateTime { get; set; }

        public bool IsDeleted { get; set; }
    }
}