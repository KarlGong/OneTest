using System.Collections.Generic;
using System.Linq;
using OneTestApi.Models;

namespace OneTestApi.Services
{
    public interface ITestCaseTagService
    {
        List<string> SearchTags(string searchText, int limit);
    }
    
    public class TestCaseTagService: ITestCaseTagService
    {
        private OneTestDbContext _context;

        public TestCaseTagService(OneTestDbContext context)
        {
            _context = context;
        }

        public List<string> SearchTags(string searchText, int limit)
        {
            return _context.TestCaseTags.Select(t => t.Value).Where(v => v.Contains(searchText)).Distinct().Take(limit).ToList();
        }
    }
}