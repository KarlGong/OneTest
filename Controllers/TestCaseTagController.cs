using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OneTestApi.Models;
using OneTestApi.Services;

namespace OneTestApi.Controllers
{
    [Route("api/tag")]
    public class TestCaseTagController: Controller
    {
        private readonly ITestCaseTagService _service;

        public TestCaseTagController(ITestCaseTagService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<List<string>> SearchTags([FromQuery] string searchText, [FromQuery] int limit)
        {
            return await _service.SearchTagsAsync(searchText, limit);
        }
    }
}