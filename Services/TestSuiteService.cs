using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OneTestApi.Models;

namespace OneTestApi.Services
{
    public class AddTestSuiteParams
    {
        public int ParentId { get; set; }

        public int Position { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }
    }
    
    public class UpdateTestSuiteParams
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
    
    public interface ITestSuiteService
    {
        Task<TestSuite> GetAsync(int id);

        Task<TestSuite> AddAsync(AddTestSuiteParams ps);

        Task<TestSuite> UpdateAsync(UpdateTestSuiteParams ps);

        Task DeleteAsync(int id);
    }

    public class TestSuiteService : ITestSuiteService
    {
        private readonly OneTestDbContext _context;

        private readonly ITestNodeService _nodeService;

        private readonly IMapper _mapper;

        public TestSuiteService(OneTestDbContext context, ITestNodeService nodeService, IMapper mapper)
        {
            _context = context;
            _nodeService = nodeService;
            _mapper = mapper;
        }

        public async Task<TestSuite> GetAsync(int id)
        {
            return await _context.TestSuites.SingleAsync(ts => ts.Id == id);
        }

        public async Task<TestSuite> AddAsync(AddTestSuiteParams ps)
        {
            var sibingsCount = await _context.TestNodes.CountAsync(tn => tn.ParentId == ps.ParentId);
            ps.Position = ps.Position <= -1 ? sibingsCount : Math.Min(ps.Position, sibingsCount);

            foreach (var node in _context.TestNodes.Where(tn =>
                tn.ParentId == ps.ParentId && tn.Position >= ps.Position))
            {
                node.Position++;
            }

            var testSuite = _mapper.Map<TestSuite>(ps);
            testSuite.Count = 0;
            
            await _context.TestSuites.AddAsync(testSuite);

            await _context.SaveChangesAsync();

            return testSuite;
        }

        public async Task<TestSuite> UpdateAsync(UpdateTestSuiteParams ps)
        {
            var previousTestSuite = await GetAsync(ps.Id);

            _mapper.Map(ps, previousTestSuite);

            await _context.SaveChangesAsync();

            return previousTestSuite;
        }

        public async Task DeleteAsync(int id)
        {
            var testSuite = await GetAsync(id);

            foreach (var node in _context.TestNodes.Where(tn =>
                tn.ParentId == testSuite.ParentId && tn.Position > testSuite.Position))
            {
                node.Position--;
            }

            testSuite.IsDeleted = true;
            (await _nodeService.GetDescendantsAsync(id)).ForEach(tn => tn.IsDeleted = true);
            
            // calc count
            foreach (var ancestor in await _nodeService.GetAncestorsAsync(id))
            {
                ancestor.Count -= testSuite.Count;
            }
            
            await _context.SaveChangesAsync();
        }
    }
}