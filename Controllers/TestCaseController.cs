using Microsoft.AspNetCore.Mvc;
using OneTestApi.Models;
using OneTestApi.Services;

namespace OneTestApi.Controllers
{
    [Route("api/case")]
    public class TestCaseController
    {
        private readonly ITestCaseService _service;

        public TestCaseController(ITestCaseService service)
        {
            _service = service;
        }
        
        [HttpGet("{id}")]
        public TestCase GetTestCase(int id)
        {
            return _service.GetTestCase(id);
        }

        [HttpPut("{id}")]
        public int AddTestCase(int id, [FromBody] AddTestCaseParams ps)
        {
            ps.TestSuiteId = id;
            return _service.AddTestCase(ps);
        }

        [HttpPost("{id}")]
        public void UpdateTestCase(int id, [FromBody] UpdateTestCaseParams ps)
        {
            ps.Id = id;
            _service.UpdateTestCase(ps);
        }

    }
}