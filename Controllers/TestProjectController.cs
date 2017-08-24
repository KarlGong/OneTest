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
        public async Task<List<TestProjectDto>> GetTestProjects()
        {
            return _mapper.Map<List<TestProjectDto>>(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<TestProjectDto> GetTestProject([FromRoute] int id)
        {
            return _mapper.Map<TestProjectDto>(await _service.GetAsync(id));
        }

        [HttpPut]
        public async Task<TestProjectDto> AddTestProject([FromBody] AddTestProjectParams ps)
        {
            return _mapper.Map<TestProjectDto>(await _service.AddAsync(ps));
        }

        [HttpPost("{id}")]
        public async Task<TestProjectDto> UpdateTestProject([FromRoute] int id, [FromBody] UpdateTestProjectParams ps)
        {
            ps.Id = id;
            return _mapper.Map<TestProjectDto>(await _service.UpdateAsync(ps));
        }

        [HttpDelete("{id}")]
        public async Task DeleteTestProject([FromRoute] int id)
        {
            await _service.DeleteAsync(id);
        }
    }
}