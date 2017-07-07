using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OneTestApi.Models;

namespace OneTestApi.Services
{
    public class AddTestCaseParams
    {
        public int TestSuiteId { get; set; }

        public string Name { get; set; }

        public string Summary { get; set; }

        public string Precondition { get; set; }

        public List<TestStep> TestSteps { get; set; } = new List<TestStep>();

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

        public List<TestStep> TestSteps { get; set; }  = new List<TestStep>();

        public class TestStep
        {
            public string Action { get; set; }

            public string ExpectedResult { get; set; }
        }
    }

    public interface ITestCaseService
    {
        TestCase GetTestCase(int testCaseId);

        List<TestSuite> GetParentTestSuites(int testCaseId);

        int AddTestCase(AddTestCaseParams ps);

        void UpdateTestCase(UpdateTestCaseParams ps);
    }

    public class TestCaseService : ITestCaseService
    {
        private OneTestDbContext _context;

        public TestCaseService(OneTestDbContext context)
        {
            _context = context;
        }


        public TestCase GetTestCase(int testCaseId)
        {
            var testCase = _context.TestCases.Include(tc => tc.TestSteps).Single(tc => tc.Id == testCaseId);

            return testCase;
        }

        public List<TestSuite> GetParentTestSuites(int testCaseId)
        {
            var parentSuites = new List<TestSuite>();
            
            var parentSuite = _context.TestCases.Include(tc => tc.TestSuite).Single(tc => tc.Id == testCaseId).TestSuite;

            while (parentSuite != null)
            {
                parentSuites.Add(parentSuite);
                parentSuite = _context.TestSuites.Include(ts => ts.ParentTestSuite)
                    .Single(ts => ts.Id == parentSuite.Id).ParentTestSuite;
            }
            parentSuites.Reverse();
            return parentSuites;
        }

        public int AddTestCase(AddTestCaseParams ps)
        {
            var testSuite = _context.TestSuites.Single(ts => ts.Id == ps.TestSuiteId);

            var newTestCase = new TestCase()
            {
                Name = ps.Name,
                Summary = ps.Summary,
                Precondition = ps.Precondition,
                Order = testSuite.Count,
                TestSteps = new List<TestStep>(
                    ps.TestSteps.Select(ts => new TestStep()
                    {
                        Action = ts.Action,
                        ExpectedResult = ts.ExpectedResult,
                    })
                )
            };

            testSuite.TestCases.Add(newTestCase);
            
            _context.SaveChanges();
            
            // update count
            GetParentTestSuites(newTestCase.Id).ForEach(ts => ts.Count++);
            
            _context.SaveChanges();

            return newTestCase.Id;
        }

        public void UpdateTestCase(UpdateTestCaseParams ps)
        {
            var testCase = _context.TestCases.Single(tc => tc.Id == ps.Id);

            testCase.Name = ps.Name;
            testCase.Summary = ps.Summary;
            testCase.Precondition = ps.Precondition;

            var oldTestSteps = _context.TestSteps.Include(ts => ts.TestCase).Where(ts => ts.TestCase.Id == ps.Id);
            foreach (var oldTestStep in oldTestSteps)
            {
                _context.TestSteps.Remove(oldTestStep);
            }

            testCase.TestSteps = new List<TestStep>(
                ps.TestSteps.Select(ts => new TestStep()
                {
                    Action = ts.Action,
                    ExpectedResult = ts.ExpectedResult,
                })
            );

            _context.SaveChanges();
        }
    }
}