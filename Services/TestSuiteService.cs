using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OneTestApi.Models;

namespace OneTestApi.Services
{
    public class AddTestSuiteParams
    {
        public int ParentSuiteId { get; set; }

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
        TestSuite GetTestSuite(int suiteId);

        TestSuite GetTestSuiteDetail(int suiteId);

        int GetChildrenCount(int testSuiteId);
        
        TestSuite GetParentTestSuite(int testSuiteId);

        TestSuite AddTestSuite(AddTestSuiteParams ps);

        void UpdateTestSuite(UpdateTestSuiteParams ps);

        void DeleteTestSuite(int testSuiteId);

        void RemovePosition(int suiteId, int pos);

        void InsertPosition(int suiteId, int pos);
    }

    public class TestSuiteService : ITestSuiteService
    {
        private OneTestDbContext _context;

        public TestSuiteService(OneTestDbContext context)
        {
            _context = context;
        }

        public TestSuite GetTestSuite(int suiteId)
        {
            return _context.TestSuites.Single(ts => ts.Id == suiteId);
        }

        public TestSuite GetTestSuiteDetail(int suiteId)
        {
            return _context.TestSuites.Include(ts => ts.TestSuites).Include(ts => ts.TestCases)
                .Single(ts => ts.Id == suiteId);
        }

        public int GetChildrenCount(int testSuiteId)
        {
            return _context.TestCases.Count(tc => tc.TestSuite.Id == testSuiteId)
                   + _context.TestSuites.Count(ts => ts.ParentTestSuite.Id == testSuiteId);
        }

        public TestSuite GetParentTestSuite(int testSuiteId)
        {
            return _context.TestSuites.Include(tc => tc.ParentTestSuite).Single(tc => tc.Id == testSuiteId)
                .ParentTestSuite;
        }

        public TestSuite AddTestSuite(AddTestSuiteParams ps)
        {
            var parentSuite = _context.TestSuites.Include(ts => ts.TestProject).Single(ts => ts.Id == ps.ParentSuiteId);

            var testSuite = new TestSuite()
            {
                Name = ps.Name,
                Description = ps.Description,
                Order = GetChildrenCount(ps.ParentSuiteId),
                ParentTestSuite = parentSuite,
                TestProject = parentSuite.TestProject
            };

            parentSuite.TestSuites.Add(testSuite);

            _context.SaveChanges();

            return testSuite;
        }

        public void UpdateTestSuite(UpdateTestSuiteParams ps)
        {
            var testSuite = _context.TestSuites.Single(ts => ts.Id == ps.Id);

            testSuite.Name = ps.Name;
            testSuite.Description = ps.Description;

            _context.SaveChanges();
        }

        public void DeleteTestSuite(int testSuiteId)
        {
            var testSuite = GetTestSuite(testSuiteId);
            
            RemovePosition(GetParentTestSuite(testSuiteId).Id, testSuite.Order);

            _context.TestSuites.Remove(testSuite);

            _context.SaveChanges();
        }

        public void RemovePosition(int suiteId, int pos)
        {
            var parenTestSuite = GetTestSuiteDetail(suiteId);
            foreach (var testCase in parenTestSuite.TestCases.Where(tc => tc.Order > pos))
            {
                testCase.Order--;
            }

            foreach (var testSuite in parenTestSuite.TestSuites.Where(tc => tc.Order > pos))
            {
                testSuite.Order--;
            }

            _context.SaveChanges();
        }

        public void InsertPosition(int suiteId, int pos)
        {
            var parenTestSuite = GetTestSuiteDetail(suiteId);
            foreach (var testCase in parenTestSuite.TestCases.Where(tc => tc.Order >= pos))
            {
                testCase.Order++;
            }

            foreach (var testSuite in parenTestSuite.TestSuites.Where(tc => tc.Order >= pos))
            {
                testSuite.Order++;
            }

            _context.SaveChanges();
        }
    }
}