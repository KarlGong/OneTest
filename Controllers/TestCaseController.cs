using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OneTestApi.Controllers.DTOs;
using OneTestApi.Models;
using OneTestApi.Services;

namespace OneTestApi.Controllers
{
    [Route("api/case")]
    public class TestCaseController
    {
        private readonly ITestCaseService _service;
        private readonly IMapper _mapper;

        public TestCaseController(ITestCaseService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        
        [HttpGet("{id}")]
        public TestCaseDto GetTestCase(int id)
        {
            return _mapper.Map<TestCaseDto>(_service.Get(id));
        }

        [HttpPut]
        public TestCaseDto AddTestCase([FromBody] AddTestCaseParams ps)
        {
            return _mapper.Map<TestCaseDto>(_service.Add(ps));
        }

        [HttpPost("{id}")]
        public TestCaseDto UpdateTestCase(int id, [FromBody] UpdateTestCaseParams ps)
        {
            ps.Id = id;
            return _mapper.Map<TestCaseDto>(_service.Update(ps));
        }
        
        [HttpPost("{id}")]
        public void MoveTestCase(int id, [FromQuery] int toParentId, [FromQuery] int toPosition)
        {
            _service.Move(id, toParentId, toPosition);
        }

        [HttpDelete("{id}")]
        public void DeleteTestCase(int id)
        {
            _service.Delete(id);
        }
    }
}