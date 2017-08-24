using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<TestSuiteDto> GetTestSuite([FromRoute] int id)
        {
            return _mapper.Map<TestSuiteDto>(await _service.GetAsync(id));
        }

        [HttpPut]
        public async Task<TestSuiteDto> AddTestSuite([FromBody] AddTestSuiteParams ps)
        {
            return _mapper.Map<TestSuiteDto>(await _service.AddAsync(ps));
        }

        [HttpPost("{id}")]
        public async Task<TestSuiteDto> UpdateTestSuite([FromRoute] int id, [FromBody] UpdateTestSuiteParams ps)
        {
            ps.Id = id;
            return _mapper.Map<TestSuiteDto>(await _service.UpdateAsync(ps));
        }

        [HttpDelete("{id}")]
        public async Task DeleteTestSuite(int id)
        {
            await _service.DeleteAsync(id);
        }
    }
}