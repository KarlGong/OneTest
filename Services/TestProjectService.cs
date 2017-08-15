using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OneTestApi.Models;

namespace OneTestApi.Services
{
    public class AddTestProjectParams
    {
        public int Position { get; set; }
        
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
        List<TestProject> GetAll();
        
        TestProject Get(int id);
        
        TestProject Add(AddTestProjectParams ps);

        TestProject Update(UpdateTestProjectParams ps);

        void Move(int id, int toPosition);

        void Delete(int id);
    }

    public class TestProjectService : ITestProjectService
    {
        private readonly OneTestDbContext _context;

        private readonly IMapper _mapper;
        
        public TestProjectService(OneTestDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<TestProject> GetAll()
        {
            return _context.TestProjects.OrderBy(tp => tp.Position).ToList();
        }

        public TestProject Get(int id)
        {
            return _context.TestProjects.Single(tp => tp.Id == id);
        }

        public TestProject Add(AddTestProjectParams ps)
        {
            var projectCount = _context.TestProjects.Count();
            ps.Position = ps.Position == -1 ? projectCount : Math.Min(ps.Position, projectCount);

            foreach (var project in _context.TestProjects.Where(tp => tp.Position >= ps.Position))
            {
                project.Position++;
            }

            var testProject = _mapper.Map<TestProject>(ps);
            
            _context.TestProjects.Add(testProject);

            _context.SaveChanges();

            return testProject;
        }

        public TestProject Update(UpdateTestProjectParams ps)
        {
            var previousTestProject = Get(ps.Id);

            _mapper.Map(ps, previousTestProject);

            _context.SaveChanges();

            return previousTestProject;
        }

        public void Move(int id, int toPosition)
        {
            var projectCount = _context.TestProjects.Count();
            toPosition = toPosition <= -1 ? projectCount : Math.Min(toPosition, projectCount);

            var previousTestProject = Get(id);

            if (previousTestProject.Position > toPosition)
            {
                foreach (var project in _context.TestProjects.Where(tp =>
                    tp.Position >= toPosition && tp.Position < previousTestProject.Position))
                {
                    project.Position++;
                }
            }
            else if (previousTestProject.Position < toPosition)
            {
                foreach (var project in _context.TestProjects.Where(tp =>
                    tp.Position <= toPosition && tp.Position > previousTestProject.Position))
                {
                    project.Position--;
                }
            }

            previousTestProject.Position = toPosition;

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var testProject = Get(id);

            foreach (var project in _context.TestProjects.Where(tp => tp.Position > testProject.Position))
            {
                project.Position--;
            }

            _context.TestProjects.Remove(testProject);

            _context.SaveChanges();
        }
    }
}