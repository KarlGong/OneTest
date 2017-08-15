using System.Collections.Generic;
using System.Linq;
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
        public TestNodeDto GetTestNode([FromRoute] int id)
        {
            return _mapper.Map<TestNodeDto>(_service.Get(id));
        }

        [HttpGet("{id}/children")]
        public List<TestNodeDto> GetChildren([FromRoute] int id)
        {
            return _service.GetChildren(id).Select(tn => _mapper.Map<TestNodeDto>(tn)).ToList();
        }
        
        [HttpPost("{id}/move")]
        public void MoveTestNode([FromRoute] int id, [FromBody] MoveTestNodeParams ps)
        {
            ps.Id = id;
            _service.Move(ps);
        }
    }
}