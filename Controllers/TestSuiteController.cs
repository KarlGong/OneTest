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

        [HttpPut("{id}")]
        public int AddTestSuite(int id, [FromBody] AddTestSuiteParams ps)
        {
            ps.ParentSuiteId = id;
            return _service.AddTestSuite(ps);
        }

    }
}