using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

namespace OneTestApi.Models
{
    public class TestCaseTag
    {
        public int Id { get; set; }
        
        [Required]
        public string Value { get; set; }
        
        public int TestCaseId { get; set; }
        
        [Required]
        [ForeignKey("TestCaseId")]
        public TestCase TestCase { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime InsertTime { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdateTime { get; set; }
        
        public bool IsDeleted { get; set; }
    }
}