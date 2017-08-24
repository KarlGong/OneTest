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
    [Route("api/node")]
    public class TestNodeController: Controller
    {
        private readonly ITestNodeService _service;
        private readonly IMapper _mapper;

        public TestNodeController(ITestNodeService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<TestNodeDto> GetTestNode([FromRoute] int id)
        {
            return _mapper.Map<TestNodeDto>(await _service.GetAsync(id));
        }

        [HttpGet("{id}/children")]
        public async Task<List<TestNodeDto>> GetChildren([FromRoute] int id)
        {
            return (await _service.GetChildrenAsync(id)).Select(tn => _mapper.Map<TestNodeDto>(tn)).ToList();
        }
        
        [HttpPost("{id}/move")]
        public async Task MoveTestNode([FromRoute] int id, [FromBody] MoveTestNodeParams ps)
        {
            ps.Id = id;
            await _service.MoveAsync(ps);
        }
    }
}