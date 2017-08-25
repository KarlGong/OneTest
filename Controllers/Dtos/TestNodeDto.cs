using System.Collections.Generic;

namespace OneTestApi.Controllers.DTOs
{
    public class TestNodeDto
    {
        public int Id { get; set; }

        public int Position { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public int? ParentId { get; set; }

        public List<TestNodeDto> Children { get; set; } = new List<TestNodeDto>();

        public int Count { get; set; }
    }
}