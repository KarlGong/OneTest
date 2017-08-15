using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OneTestApi.Controllers.DTOs;
using OneTestApi.Models;
using OneTestApi.Services;

namespace OneTestApi.Controllers
{
    [Route("api/case")]
    public class TestCaseController: Controller
    {
        private readonly ITestCaseService _service;
        private readonly IMapper _mapper;

        public TestCaseController(ITestCaseService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public TestCaseDto GetTestCase([FromRoute] int id)
        {
            return _mapper.Map<TestCaseDto>(_service.Get(id));
        }

        [HttpPut]
        public TestCaseDto AddTestCase([FromBody] AddTestCaseParams ps)
        {
            return _mapper.Map<TestCaseDto>(_service.Add(ps));
        }

        [HttpPost("{id}")]
        public TestCaseDto UpdateTestCase([FromRoute] int id, [FromBody] UpdateTestCaseParams ps)
        {
            ps.Id = id;
            return _mapper.Map<TestCaseDto>(_service.Update(ps));
        }

        [HttpDelete("{id}")]
        public void DeleteTestCase([FromRoute] int id)
        {
            _service.Delete(id);
        }
    }
}