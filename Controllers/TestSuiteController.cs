using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OneTestApi.Models;
using OneTestApi.Services;

namespace OneTestApi.Controllers
{
    public class TestNode
    {
        public string Type { get; set; }
        
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public int Order { get; set; }
        
        public int? Count { get; set; }
        
        public List<TestNode> Children { get; set; }= new List<TestNode>();
    }

    [Route("api/suite")]
    public class TestSuiteController
    {
        private readonly ITestSuiteService _service;

        public TestSuiteController(ITestSuiteService service)
        {
            _service = service;
        }
        
        [HttpGet("{id}")]
        public TestSuite GetTestSuite(int id)
        {
            return _service.GetTestSuite(id);
        }

        [HttpGet("root")]
        public TestNode GetRootTestSuite([FromQuery] int projectId)
        {
            var testSuite = _service.GetRootTestSuite(projectId);
            return new TestNode()
            {
                Type = "rootSuite",
                Id = testSuite.Id,
                Name = testSuite.Name,
                Order = testSuite.Order,
                Count = testSuite.Count
            };
        }

        [HttpGet("{id}/children")]
        public IEnumerable<TestNode> GetChildren(int id)
        {
            var testSuite = _service.GetTestSuiteDetail(id);
            return testSuite.TestCases.Select(tc => new TestNode()
            {
                Type = "case",
                Id = tc.Id,
                Name = tc.Name,
                Order = tc.Order,
                Count = null
            }).Union(testSuite.TestSuites.Select(ts => new TestNode()
            {
                Type = "suite",
                Id = ts.Id,
                Name = ts.Name,
                Order = ts.Order,
                Count = ts.Count
            })).OrderBy(tn => tn.Order);
        }

        [HttpPut]
        public int AddTestSuite([FromBody] AddTestSuiteParams ps)
        {
            return _service.AddTestSuite(ps);
        }

        [HttpPost("{id}")]
        public void UpdateTestSuite(int id, [FromBody] UpdateTestSuiteParams ps)
        {
            ps.Id = id;
            _service.UpdateTestSuite(ps);
        }

    }
}