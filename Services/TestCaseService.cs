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

        public ExecutionType ExecutionType { get; set; }

        public Importance Importance { get; set; }

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

        public ExecutionType ExecutionType { get; set; }

        public Importance Importance { get; set; }

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

    public class MoveTestCaseParams
    {
        public int Id { get; set; }
        
        public int TestSuiteId { get; set; }
        
        public int Position { get; set; }
    }

    public interface ITestCaseService
    {
        TestCase GetTestCase(int testCaseId);

        TestCase AddTestCase(AddTestCaseParams ps);
        
        TestSuite GetParentTestSuite(int testCaseId);

        void UpdateTestCase(UpdateTestCaseParams ps);

        void DeleteTestCase(int testCaseId);

        void MoveTestCase(MoveTestCaseParams ps);
        
    }

    public class TestCaseService : ITestCaseService
    {
        private OneTestDbContext _context;
        private ITestSuiteService _suiteService;

        public TestCaseService(OneTestDbContext context, ITestSuiteService suiteService)
        {
            _context = context;
            _suiteService = suiteService;
        }


        public TestCase GetTestCase(int testCaseId)
        {
            var testCase = _context.TestCases.Include(tc => tc.Tags).Single(tc => tc.Id == testCaseId);

            testCase.TestSteps = _context.TestSteps.Where(ts => ts.TestCase.Id == testCaseId).OrderBy(ts => ts.Id).ToList();
            
            return testCase;
        }

        public TestCase AddTestCase(AddTestCaseParams ps)
        {
            var testSuite = _context.TestSuites.Single(ts => ts.Id == ps.TestSuiteId);
            
            var newTestCase = new TestCase()
            {
                Name = ps.Name,
                Summary = ps.Summary,
                Precondition = ps.Precondition,
                ExecutionType = ps.ExecutionType,
                Importance = ps.Importance,
                Order = _suiteService.GetChildrenCount(ps.TestSuiteId),
                Tags = new List<TestCaseTag>(
                    ps.Tags.Select(t => new TestCaseTag()
                    {
                        Value = t.Value
                    })
                ),
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

            return newTestCase;
        }

        public TestSuite GetParentTestSuite(int testCaseId)
        {
            return _context.TestCases.Include(tc => tc.TestSuite).Single(tc => tc.Id == testCaseId)
                .TestSuite;
        }

        public void UpdateTestCase(UpdateTestCaseParams ps)
        {
            var testCase = _context.TestCases.Single(tc => tc.Id == ps.Id);

            testCase.Name = ps.Name;
            testCase.Summary = ps.Summary;
            testCase.Precondition = ps.Precondition;
            testCase.ExecutionType = ps.ExecutionType;
            testCase.Importance = ps.Importance;

            var oldTags = _context.TestCaseTags.Where(t => t.TestCase.Id == ps.Id);
            _context.TestCaseTags.RemoveRange(oldTags);
            
            testCase.Tags = new List<TestCaseTag>(
                ps.Tags.Select(t => new TestCaseTag()
                {
                    Value = t.Value
                })
            );

            var oldTestSteps = _context.TestSteps.Where(ts => ts.TestCase.Id == ps.Id);
            _context.TestSteps.RemoveRange(oldTestSteps);

            testCase.TestSteps = new List<TestStep>(
                ps.TestSteps.Select(ts => new TestStep()
                {
                    Action = ts.Action,
                    ExpectedResult = ts.ExpectedResult,
                })
            );

            _context.SaveChanges();
        }

        public void DeleteTestCase(int testCaseId)
        {
            var testCase = GetTestCase(testCaseId);
            
            _suiteService.RemovePosition(GetParentTestSuite(testCaseId).Id, testCase.Order);

            _context.TestCases.Remove(testCase);

            _context.SaveChanges();
        }

        public void MoveTestCase(MoveTestCaseParams ps)
        {
            var testCase = GetTestCase(ps.Id);

            _suiteService.RemovePosition(GetParentTestSuite(testCase.Id).Id, testCase.Order);

            if (ps.Position == -1)
            {
                testCase.TestSuite = _suiteService.GetTestSuite(ps.TestSuiteId);

                testCase.Order = _suiteService.GetChildrenCount(ps.TestSuiteId);

                _context.SaveChanges();
            }
            else
            {
                _suiteService.InsertPosition(ps.TestSuiteId, ps.Position);

                testCase.TestSuite = _suiteService.GetTestSuite(ps.TestSuiteId);

                testCase.Order = ps.Position;

                _context.SaveChanges();
            }
        }
    }
}