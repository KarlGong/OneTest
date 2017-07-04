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

        public List<TestStep> TestSteps { get; set; }

        public class TestStep
        {
            public string Value { get; set; }

            public bool IsVerification { get; set; }
        }
    }

    public class UpdateTestCaseParams
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Summary { get; set; }

        public string Precondition { get; set; }

        public List<TestStep> TestSteps { get; set; }

        public class TestStep
        {
            public string Value { get; set; }

            public bool IsVerification { get; set; }
        }
    }

    public interface ITestCaseService
    {
        TestCase GetTestCase(int testCaseId);

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
                        Value = ts.Value,
                        IsVerification = ts.IsVerification,
                    })
                )
            };

            testSuite.TestCases.Add(newTestCase);

            testSuite.Count++;

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
                    Value = ts.Value,
                    IsVerification = ts.IsVerification,
                })
            );

            _context.SaveChanges();
        }
    }
}