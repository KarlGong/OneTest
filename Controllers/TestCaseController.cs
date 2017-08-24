using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<TestCaseDto> GetTestCase([FromRoute] int id)
        {
            return _mapper.Map<TestCaseDto>(await _service.GetAsync(id));
        }

        [HttpPut]
        public async Task<TestCaseDto> AddTestCase([FromBody] AddTestCaseParams ps)
        {
            return _mapper.Map<TestCaseDto>(await _service.AddAsync(ps));
        }

        [HttpPost("{id}")]
        public async Task<TestCaseDto> UpdateTestCase([FromRoute] int id, [FromBody] UpdateTestCaseParams ps)
        {
            ps.Id = id;
            return _mapper.Map<TestCaseDto>(await _service.UpdateAsync(ps));
        }

        [HttpDelete("{id}")]
        public async Task DeleteTestCase([FromRoute] int id)
        {
            await _service.DeleteAsync(id);
        }
    }
}