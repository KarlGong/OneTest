using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OneTestApi.Models;
using OneTestApi.Services;

namespace OneTestApi.Controllers
{
    [Route("api/tag")]
    public class TestCaseTagController
    {
        private readonly ITestCaseTagService _service;

        public TestCaseTagController(ITestCaseTagService service)
        {
            _service = service;
        }

        [HttpGet]
        public List<string> SearchTags([FromQuery] string searchText, [FromQuery] int limit)
        {
            return _service.SearchTags(searchText, limit);
        }
    }
}