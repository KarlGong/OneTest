using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OneTestApi.Models
{
    public class TestStep
    {
        public int Id { get; set; }
        
        public string Action { get; set; }
        
        public string ExpectedResult { get; set; }
        
        public int TestCaseId { get; set; }
        
        [Required]
        [ForeignKey("TestCaseId")]
        public TestCase TestCase { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime InsertTime { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdateTime { get; set; }
    }
}