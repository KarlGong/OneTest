using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OneTestApi.Models;

namespace OneTestApi.Services
{
    public class AddTestCaseParams
    {
        public int ParentId { get; set; }

        public int Position { get; set; }

        public string Name { get; set; }

        public string Summary { get; set; }

        public string Precondition { get; set; }

        public TestCaseExecutionType ExecutionType { get; set; }

        public TestCaseImportance Importance { get; set; }

        public List<TestCaseTag> Tags { get; set; } = new List<TestCaseTag>();

        public List<TestStep> TestSteps { get; set; } = new List<TestStep>();

        public class TestCaseTag
        {
            public string Value { get; set; }
        }

        public class TestStep
        {
            public string Action { get; set; }

            public string ExpectedResult { get; set; }
        }
    }

    public class UpdateTestCaseParams
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Summary { get; set; }

        public string Precondition { get; set; }

        public TestCaseExecutionType ExecutionType { get; set; }

        public TestCaseImportance Importance { get; set; }

        public List<TestCaseTag> Tags { get; set; } = new List<TestCaseTag>();

        public List<TestStep> TestSteps { get; set; } = new List<TestStep>();

        public class TestCaseTag
        {
            public string Value { get; set; }
        }

        public class TestStep
        {
            public string Action { get; set; }

            public string ExpectedResult { get; set; }
        }
    }

    public interface ITestCaseService
    {
        Task<TestCase> GetAsync(int id);

        Task<TestCase> AddAsync(AddTestCaseParams ps);

        Task<TestCase> UpdateAsync(UpdateTestCaseParams ps);

        Task DeleteAsync(int id);
    }

    public class TestCaseService : ITestCaseService
    {
        private readonly OneTestDbContext _context;

        private readonly IMapper _mapper;

        public TestCaseService(OneTestDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TestCase> GetAsync(int id)
        {
            var testCase = await _context.TestCases.Include(tc => tc.TestSteps).Include(tc => tc.Tags)
                .SingleAsync(tc => tc.Id == id);
            testCase.TestSteps = testCase.TestSteps.OrderBy(ts => ts.Id).ToList();

            return testCase;
        }

        public async Task<TestCase> AddAsync(AddTestCaseParams ps)
        {
            var sibingsCount = await _context.TestNodes.CountAsync(tn => tn.ParentId == ps.ParentId);
            ps.Position = ps.Position == -1 ? sibingsCount : Math.Min(ps.Position, sibingsCount);

            foreach (var node in _context.TestNodes.Where(tn =>
                tn.ParentId == ps.ParentId && tn.Position >= ps.Position))
            {
                node.Position++;
            }

            var testCase = _mapper.Map<TestCase>(ps);
            
            await _context.TestCases.AddAsync(testCase);

            await _context.SaveChangesAsync();

            return testCase;
        }

        public async Task<TestCase> UpdateAsync(UpdateTestCaseParams ps)
        {
            var previousTestCase = await GetAsync(ps.Id);

            _context.TestCases.Attach(previousTestCase);

            _mapper.Map(ps, previousTestCase);

            await _context.SaveChangesAsync();

            return previousTestCase;
        }

        public async Task DeleteAsync(int id)
        {
            var testCase = await GetAsync(id);

            foreach (var node in _context.TestNodes.Where(tn =>
                tn.ParentId == testCase.ParentId && tn.Position > testCase.Position))
            {
                node.Position--;
            }

            _context.TestCases.Remove(testCase);

            await _context.SaveChangesAsync();
        }
    }
}