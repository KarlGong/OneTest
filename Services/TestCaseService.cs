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

        public string ExecutionType { get; set; }
        
        public string Importance { get; set; }
        
        public List<AddTestCaseTagParams> Tags { get; set; } = new List<AddTestCaseTagParams>();

        public List<AddTestStepParams> TestSteps { get; set; } = new List<AddTestStepParams>();
    }

    public class AddTestCaseTagParams
    {
        public string Value { get; set; }
    }

    public class AddTestStepParams
    {
        public string Action { get; set; }
        
        public string ExpectedResult { get; set; }
    }
    
    public class UpdateTestCaseParams
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string Summary { get; set; }
        
        public string Precondition { get; set; }

        public string ExecutionType { get; set; }
        
        public string Importance { get; set; }
        
        public List<UpdateTestCaseTagParams> Tags { get; set; } = new List<UpdateTestCaseTagParams>();

        public List<UpdateTestStepParams> TestSteps { get; set; } = new List<UpdateTestStepParams>();
    }

    public class UpdateTestCaseTagParams
    {
        public string Value { get; set; }
    }

    public class UpdateTestStepParams
    {
        public string Action { get; set; }
        
        public string ExpectedResult { get; set; }
    }
    
    public interface ITestCaseService
    {
        TestCase Get(int id);

        TestNode GetParent(int id);

        TestCase Add(AddTestCaseParams ps);

        TestCase Update(UpdateTestCaseParams ps);

        void Move(int id, int toParentId, int toPosition);

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

        public TestNode GetParent(int id)
        {
            return _context.TestCases.Include(tn => tn.Parent).Single(tn => tn.Id == id).Parent;
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

            _context.TestSteps.RemoveRange(previousTestCase.TestSteps);
            
            _context.TestCaseTags.RemoveRange(previousTestCase.Tags);

            _mapper.Map(ps, previousTestCase);

            _context.SaveChanges();

            return previousTestCase;
        }

        public void Move(int id, int toParentId, int toPosition)
        {
            var sibingsCount = _context.TestNodes.Count(tn => tn.ParentId == toParentId);
            toPosition = toPosition <= -1 ? sibingsCount : Math.Min(toPosition, sibingsCount);
            
            var previousTestCase = Get(id);

            if (previousTestCase.ParentId == toParentId)
            {
                if (previousTestCase.Position > toPosition)
                {
                    foreach (var node in _context.TestNodes.Where(tn =>
                        tn.ParentId == toParentId && tn.Position >= toPosition &&
                        tn.Position < previousTestCase.Position))
                    {
                        node.Position++;
                    }
                }
                else if (previousTestCase.Position < toPosition)
                {
                    foreach (var node in _context.TestNodes.Where(tn =>
                        tn.ParentId == toParentId && tn.Position <= toPosition &&
                        tn.Position > previousTestCase.Position))
                    {
                        node.Position--;
                    }
                }
            }
            else
            {
                foreach (var node in _context.TestNodes.Where(tn =>
                    tn.ParentId == previousTestCase.ParentId && tn.Position > previousTestCase.Position))
                {
                    node.Position--;
                }

                foreach (var node in _context.TestNodes.Where(tn =>
                    tn.ParentId == toParentId && tn.Position >= toPosition))
                {
                    node.Position++;
                }
            }

            previousTestCase.ParentId = toParentId;
            previousTestCase.Position = toPosition;
            
            _context.SaveChanges();
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