using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OneTestApi.Models
{
    public class TestSuite : TestNode
    {
        public string Description { get; set; }
    }
}