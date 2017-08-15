using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OneTestApi.Models;

namespace OneTestApi.Services
{
    public interface ITestNodeService
    {
        TestNode Get(int id);
        
        TestNode GetParent(int id);

        List<TestNode> GetChildren(int id);
    }

    public class TestNodeService : ITestNodeService
    {
        private readonly OneTestDbContext _context;

        public TestNodeService(OneTestDbContext context)
        {
            _context = context;
        }

        public TestNode Get(int id)
        {
            return _context.TestNodes.Single(tn => tn.Id == id);
        }

        public TestNode GetParent(int id)
        {
            return _context.TestNodes.Include(tn => tn.Parent).Single(tn => tn.Id == id).Parent;
        }

        public List<TestNode> GetChildren(int id)
        {
            return _context.TestNodes.Include(ts => ts.Children).Single(ts => ts.Id == id).Children;
        }
    }
}