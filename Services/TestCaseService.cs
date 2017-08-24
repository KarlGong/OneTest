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
        TestCase Get(int id);

        TestCase Add(AddTestCaseParams ps);

        TestCase Update(UpdateTestCaseParams ps);

        void Delete(int id);
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

        public TestCase Get(int id)
        {
            var testCase = _context.TestCases.Include(tc => tc.TestSteps).Include(tc => tc.Tags)
                .Single(tc => tc.Id == id);
            testCase.TestSteps = testCase.TestSteps.OrderBy(ts => ts.Id).ToList();

            return testCase;
        }

        public TestCase Add(AddTestCaseParams ps)
        {
            var sibingsCount = _context.TestNodes.Count(tn => tn.ParentId == ps.ParentId);
            ps.Position = ps.Position == -1 ? sibingsCount : Math.Min(ps.Position, sibingsCount);

            foreach (var node in _context.TestNodes.Where(tn =>
                tn.ParentId == ps.ParentId && tn.Position >= ps.Position))
            {
                node.Position++;
            }

            var testCase = _mapper.Map<TestCase>(ps);
            
            _context.TestCases.Add(testCase);

            _context.SaveChanges();

            return testCase;
        }

        public TestCase Update(UpdateTestCaseParams ps)
        {
            var previousTestCase = Get(ps.Id);

            _context.TestCases.Attach(previousTestCase);

            _mapper.Map(ps, previousTestCase);

            _context.SaveChanges();

            return previousTestCase;
        }

        public void Delete(int id)
        {
            var testCase = Get(id);

            foreach (var node in _context.TestNodes.Where(tn =>
                tn.ParentId == testCase.ParentId && tn.Position > testCase.Position))
            {
                node.Position--;
            }

            _context.TestCases.Remove(testCase);

            _context.SaveChanges();
        }
    }
}