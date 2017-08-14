using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OneTestApi.Models
{
    public class TestProject : TestNode
    {
        public string Description { get; set; }
    }
}