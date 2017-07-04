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


    public interface ITestSuiteService
    {
        TestSuite GetTestSuite(int suiteId);

        int AddTestSuite(AddTestSuiteParams ps);
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

        public int AddTestSuite(AddTestSuiteParams ps)
        {
            var parent = _context.TestSuites.Include(ts => ts.TestProject).Single(ts => ts.Id == ps.ParentSuiteId);

            var testSuite = new TestSuite()
            {
                Name = ps.Name,
                Description = ps.Description,
                Order = 0,
                Count = 0,
                ParentTestSuite = parent,
                TestProject = parent.TestProject
            };

            parent.TestSuites.Add(testSuite);

            _context.SaveChanges();

            return testSuite.Id;
        }
    }
}