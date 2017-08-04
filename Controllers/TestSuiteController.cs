using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OneTestApi.Models;
using OneTestApi.Services;

namespace OneTestApi.Controllers
{
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

        [HttpGet("{id}/children")]
        public IEnumerable<TestNode> GetChildren(int id)
        {
            var testSuite = _service.GetTestSuiteDetail(id);
            return testSuite.TestCases.Select(tc => new TestNode()
            {
                Type = "case",
                Id = tc.Id,
                Name = tc.Name,
                Order = tc.Order
            }).Union(testSuite.TestSuites.Select(ts => new TestNode()
            {
                Type = "suite",
                Id = ts.Id,
                Name = ts.Name,
                Order = ts.Order
            })).OrderBy(tn => tn.Order);
        }

        [HttpPut]
        public TestSuite AddTestSuite([FromBody] AddTestSuiteParams ps)
        {
            return _service.AddTestSuite(ps);
        }

        [HttpPost("{id}")]
        public void UpdateTestSuite(int id, [FromBody] UpdateTestSuiteParams ps)
        {
            ps.Id = id;
            _service.UpdateTestSuite(ps);
        }

        [HttpDelete("{id}")]
        public void DeleteTestSuite(int id)
        {
            _service.DeleteTestSuite(id);
        }

    }
}