using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using OneTestApi.Controllers.DTOs;
using OneTestApi.Models;
using OneTestApi.Services;

namespace OneTestApi.Controllers
{
    [Route("api/project")]
    public class TestProjectController: Controller
    {
        private readonly ITestProjectService _service;
        private readonly IMapper _mapper;

        public TestProjectController(ITestProjectService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("all")]
        public IEnumerable<TestProjectDto> GetTestProjects()
        {
            return _mapper.Map<List<TestProjectDto>>(_service.GetAll());
        }

        [HttpGet("{id}")]
        public TestProjectDto GetTestProject([FromRoute] int id)
        {
            return _mapper.Map<TestProjectDto>(_service.Get(id));
        }

        [HttpPut]
        public TestProjectDto AddTestProject([FromBody] AddTestProjectParams ps)
        {
            return _mapper.Map<TestProjectDto>(_service.Add(ps));
        }

        [HttpPost("{id}")]
        public TestProjectDto UpdateTestProject([FromRoute] int id, [FromBody] UpdateTestProjectParams ps)
        {
            ps.Id = id;
            return _mapper.Map<TestProjectDto>(_service.Update(ps));
        }

        [HttpDelete("{id}")]
        public void DeleteTestProject([FromRoute] int id)
        {
            _service.Delete(id);
        }
    }
}