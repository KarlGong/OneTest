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
    public class TestProjectController
    {
        private readonly ITestProjectService _service;
        private readonly IMapper _mapper;

        public TestProjectController(ITestProjectService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<TestProjectDto> GetTestProjects()
        {
            return _mapper.Map<List<TestProjectDto>>(_service.GetAll());
        }

        [HttpGet("{id}")]
        public TestProjectDto GetTestProject([FromRoute] int id)
        {
            return _mapper.Map<TestProjectDto>(_service.Get(id));
        }

        [HttpGet("{id}/children")]
        public List<object> GetChildren([FromRoute] int id)
        {
            var children = new List<object>();
            foreach (var testNode in _service.GetChildren(id))
            {
                if (testNode is TestCase)
                {
                    children.Add(_mapper.Map<TestCaseDto>(testNode));
                }
                else if (testNode is TestSuite)
                {
                    children.Add(_mapper.Map<TestSuiteDto>(testNode));
                }
            }
            return children;
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

        [HttpPost("{id}/move")]
        public void MoveTestProject([FromRoute] int id, [FromBody] int toPosition)
        {
            _service.Move(id, toPosition);
        }

        [HttpDelete("{id}")]
        public void DeleteTestProject([FromRoute] int id)
        {
            _service.Delete(id);
        }
    }
}