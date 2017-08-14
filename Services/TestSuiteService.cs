using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OneTestApi.Models;

namespace OneTestApi.Services
{
    public class AddTestSuiteParams
    {
        public int ParentId { get; set; }

        public int Position { get; set; }
        
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
        TestSuite Get(int id);

        TestNode GetParent(int id);

        List<TestNode> GetChildren(int id);

        TestSuite Add(AddTestSuiteParams ps);

        TestSuite Update(UpdateTestSuiteParams ps);

        void Move(int id, int toParentId, int toPosition);
        
        void Delete(int id);
    }

    public class TestSuiteService : ITestSuiteService
    {
        private readonly OneTestDbContext _context;

        private readonly IMapper _mapper;

        public TestSuiteService(OneTestDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public TestSuite Get(int id)
        {
            return _context.TestSuites.Single(ts => ts.Id == id);
        }

        public TestNode GetParent(int id)
        {
            return _context.TestSuites.Include(tn => tn.Parent).Single(tn => tn.Id == id).Parent;
        }

        public List<TestNode> GetChildren(int id)
        {
            return _context.TestSuites.Include(ts => ts.Children).Single(ts => ts.Id == id).Children;
        }

        public TestSuite Add(AddTestSuiteParams ps)
        {
            var sibingsCount = _context.TestNodes.Count(tn => tn.ParentId == ps.ParentId);
            ps.Position = ps.Position == -1 ? sibingsCount : Math.Min(ps.Position, sibingsCount);

            foreach (var node in _context.TestNodes.Where(tn =>
                tn.ParentId == ps.ParentId && tn.Position >= ps.Position))
            {
                node.Position++;
            }

            var testSuite = _mapper.Map<TestSuite>(ps);
            
            _context.TestSuites.Add(testSuite);

            _context.SaveChanges();

            return testSuite;
        }

        public TestSuite Update(UpdateTestSuiteParams ps)
        {
            var previousTestSuite = Get(ps.Id);

            _mapper.Map(ps, previousTestSuite);

            _context.SaveChanges();

            return previousTestSuite;
        }

        public void Move(int id, int toParentId, int toPosition)
        {
            var sibingsCount = _context.TestNodes.Count(tn => tn.ParentId == toParentId);
            toPosition = toPosition <= -1 ? sibingsCount : Math.Min(toPosition, sibingsCount);

            var previousTestSuite = Get(id);

            if (previousTestSuite.ParentId == toParentId)
            {
                if (previousTestSuite.Position > toPosition)
                {
                    foreach (var node in _context.TestNodes.Where(tn =>
                        tn.ParentId == toParentId && tn.Position >= toPosition &&
                        tn.Position < previousTestSuite.Position))
                    {
                        node.Position++;
                    }
                }
                else if (previousTestSuite.Position < toPosition)
                {
                    foreach (var node in _context.TestNodes.Where(tn =>
                        tn.ParentId == toParentId && tn.Position <= toPosition &&
                        tn.Position > previousTestSuite.Position))
                    {
                        node.Position--;
                    }
                }
            }
            else
            {
                foreach (var node in _context.TestNodes.Where(tn =>
                    tn.ParentId == previousTestSuite.ParentId && tn.Position > previousTestSuite.Position))
                {
                    node.Position--;
                }

                foreach (var node in _context.TestNodes.Where(tn =>
                    tn.ParentId == toParentId && tn.Position >= toPosition))
                {
                    node.Position++;
                }
            }

            previousTestSuite.ParentId = toParentId;
            previousTestSuite.Position = toPosition;

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var testSuite = Get(id);

            foreach (var node in _context.TestNodes.Where(tn =>
                tn.ParentId == testSuite.ParentId && tn.Position > testSuite.Position))
            {
                node.Position--;
            }

            _context.TestSuites.Remove(testSuite);

            _context.SaveChanges();
        }
    }
}