using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OneTestApi.Models;

namespace OneTestApi.Services
{
    public class AddTestProjectParams
    {
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
        IEnumerable<TestProject> GetTestProjects();

        TestProject GetTestProject(int projectId);

        TestSuite GetRootTestSuite(int projectId);

        int AddTestProject(AddTestProjectParams ps);

        void UpdateTestProject(UpdateTestProjectParams ps);
    }

    public class TestProjectService : ITestProjectService
    {
        private readonly OneTestDbContext _context;

        public TestProjectService(OneTestDbContext context)
        {
            _context = context;
        }

        
        public IEnumerable<TestProject> GetTestProjects()
        {
            return _context.TestProjects.ToList();
        }

        public TestProject GetTestProject(int projectId)
        {
            return _context.TestProjects.Single(tp => tp.Id == projectId);
        }
        
        public TestSuite GetRootTestSuite(int projectId)
        {
            return _context.TestProjects.Include(tp => tp.TestSuites)
                .Single(tp => tp.Id == projectId).TestSuites.First();
        }

        public int AddTestProject(AddTestProjectParams ps)
        {
            var testProject = new TestProject()
            {
                Name = ps.Name,
                Description = ps.Description,
                TestSuites = new List<TestSuite>()
                {
                    new TestSuite()
                    {
                        Name = ps.Name,
                        Description = "Root test suite",
                        Order = 0,
                    }
                }
            };

            _context.TestProjects.Add(testProject);

            _context.SaveChanges();

            return testProject.Id;
        }

        public void UpdateTestProject(UpdateTestProjectParams ps)
        {
            var testProject = _context.TestProjects.Include(tp => tp.TestSuites).Single(tp => tp.Id == ps.Id);

            testProject.Name = ps.Name;
            testProject.Description = ps.Description;

            _context.SaveChanges();
        }
    }
}