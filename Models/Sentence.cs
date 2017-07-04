using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OneTestApi.Models
{
    public class Sentence
    {
        public int SentenceId { get; set; }
        
        public string Value { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime InsertTime { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdateTime { get; set; }
    }
}