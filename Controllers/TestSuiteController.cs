using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OneTestApi.Controllers.DTOs;
using OneTestApi.Models;
using OneTestApi.Services;

namespace OneTestApi.Controllers
{
    [Route("api/suite")]
    public class TestSuiteController
    {
        private readonly ITestSuiteService _service;
        private readonly IMapper _mapper;

        public TestSuiteController(ITestSuiteService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public TestSuiteDto GetTestSuite([FromRoute] int id)
        {
            return _mapper.Map<TestSuiteDto>(_service.Get(id));
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
        public TestSuiteDto AddTestSuite([FromBody] AddTestSuiteParams ps)
        {
            return _mapper.Map<TestSuiteDto>(_service.Add(ps));
        }

        [HttpPost("{id}")]
        public TestSuiteDto UpdateTestSuite([FromRoute] int id, [FromBody] UpdateTestSuiteParams ps)
        {
            ps.Id = id;
            return _mapper.Map<TestSuiteDto>(_service.Update(ps));
        }

        [HttpPost("{id}")]
        public void MoveTestSuite([FromRoute] int id, [FromBody] int toParentId, [FromBody] int toPosition)
        {
            _service.Move(id, toParentId, toPosition);
        }

        [HttpDelete("{id}")]
        public void DeleteTestSuite(int id)
        {
            _service.Delete(id);
        }
    }
}