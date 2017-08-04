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

        [HttpPut]
        public TestCase AddTestCase([FromBody] AddTestCaseParams ps)
        {
            return _service.AddTestCase(ps);
        }

        [HttpPost("{id}")]
        public void UpdateTestCase(int id, [FromBody] UpdateTestCaseParams ps)
        {
            ps.Id = id;
            _service.UpdateTestCase(ps);
        }

        [HttpDelete("{id}")]
        public void DeleteTestCase(int id)
        {
            _service.DeleteTestCase(id);
        }

        [HttpPost("{id}/move")]
        public void MoveTestCase(int id, [FromBody] MoveTestCaseParams ps)
        {
            ps.Id = id;
            _service.MoveTestCase(ps);
        }
    }
}