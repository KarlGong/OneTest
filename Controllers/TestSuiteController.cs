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
        public IEnumerable<object> GetChildren(int id)
        {
            var testSuite = _service.GetTestSuiteDetail(id);
            return testSuite.TestCases.Select(tc => new
            {
                Type = "TestCase",
                tc.Id,
                tc.Name,
                tc.Order
            }).Union(testSuite.TestSuites.Select(ts => new
            {
                Type = "TestSuite",
                ts.Id,
                ts.Name,
                ts.Order
            })).OrderBy(tn => tn.Order);
        }

        [HttpPut("{id}")]
        public int AddTestSuite(int id, [FromBody] AddTestSuiteParams ps)
        {
            ps.ParentSuiteId = id;
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