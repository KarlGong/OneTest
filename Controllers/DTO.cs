using System.Collections.Generic;

namespace OneTestApi.Controllers
{
    public class TestNode
    {
        public string Type { get; set; }
        
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public int Order { get; set; }
        
        public List<TestNode> Children { get; set; }= new List<TestNode>();
    }
}