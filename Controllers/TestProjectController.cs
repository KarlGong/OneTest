using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using OneTestApi.Models;
using OneTestApi.Services;

namespace OneTestApi.Controllers
{
    [Route("api/project")]
    public class TestProjectController
    {
        private readonly ITestProjectService _service;

        public TestProjectController(ITestProjectService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<TestProject> GetTestProjects()
        {
            return _service.GetTestProjects();
        }
        
        [HttpGet("{id}")]
        public TestProject GetTestProject(int id)
        {
            return _service.GetTestProject(id);
        }

        [HttpPut]
        public int AddTestProject([FromBody] AddTestProjectParams ps)
        {
            return _service.AddTestProject(ps);
        }

        [HttpPost("{id}")]
        public void UpdateTestProject(int id, [FromBody] UpdateTestProjectParams ps)
        {
            ps.Id = id;
            _service.UpdateTestProject(ps);
        }
    }
}