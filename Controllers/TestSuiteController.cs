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
    public class TestSuiteController: Controller
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

        [HttpDelete("{id}")]
        public void DeleteTestSuite(int id)
        {
            _service.Delete(id);
        }
    }
}