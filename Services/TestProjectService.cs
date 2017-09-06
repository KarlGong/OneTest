using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OneTestApi.Models;

namespace OneTestApi.Services
{
    public class AddTestProjectParams
    {
        public int Position { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }
    }
    
    public class UpdateTestProjectParams
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }
    }
    
    public interface ITestProjectService
    {
        Task<List<TestProject>> GetAllAsync();
        
        Task<TestProject> GetAsync(int id);
        
        Task<TestProject> AddAsync(AddTestProjectParams ps);

        Task<TestProject> UpdateAsync(UpdateTestProjectParams ps);

        Task DeleteAsync(int id);
    }

    public class TestProjectService : ITestProjectService
    {
        private readonly OneTestDbContext _context;
        
        private readonly ITestNodeService _nodeService;

        private readonly IMapper _mapper;
        
        public TestProjectService(OneTestDbContext context, ITestNodeService nodeService, IMapper mapper)
        {
            _context = context;
            _nodeService = nodeService;
            _mapper = mapper;
        }

        public async Task<List<TestProject>> GetAllAsync()
        {
            return await _context.TestProjects.OrderBy(tp => tp.Position).ToListAsync();
        }

        public async Task<TestProject> GetAsync(int id)
        {
            return await _context.TestProjects.SingleAsync(tp => tp.Id == id);
        }

        public async Task<TestProject> AddAsync(AddTestProjectParams ps)
        {
            var projectCount = await _context.TestProjects.CountAsync();
            ps.Position = ps.Position <= -1 ? projectCount : Math.Min(ps.Position, projectCount);

            foreach (var project in _context.TestProjects.Where(tp => tp.Position >= ps.Position))
            {
                project.Position++;
            }

            var testProject = _mapper.Map<TestProject>(ps);
            testProject.Count = 0;
            
            await _context.TestProjects.AddAsync(testProject);

            await _context.SaveChangesAsync();

            return testProject;
        }

        public async Task<TestProject> UpdateAsync(UpdateTestProjectParams ps)
        {
            var previousTestProject = await GetAsync(ps.Id);

            _mapper.Map(ps, previousTestProject);

            await _context.SaveChangesAsync();

            return previousTestProject;
        }

        public async Task DeleteAsync(int id)
        {
            var testProject = await GetAsync(id);

            foreach (var project in _context.TestProjects.Where(tp => tp.Position > testProject.Position))
            {
                project.Position--;
            }

            testProject.IsDeleted = true;
            (await _nodeService.GetDescendantsAsync(id)).ForEach(tn => tn.IsDeleted = true);

            await _context.SaveChangesAsync();
        }
    }
}