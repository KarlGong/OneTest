using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OneTestApi.Models;

namespace OneTestApi.Services
{
    public interface ITestCaseTagService
    {
        Task<List<string>> SearchTagsAsync(string searchText, int limit);
    }

    public class TestCaseTagService : ITestCaseTagService
    {
        private OneTestDbContext _context;

        public TestCaseTagService(OneTestDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> SearchTagsAsync(string searchText = "", int limit = 10)
        {
            return await _context.TextCaseTags.Select(t => t.Value).Where(v => v.Contains(searchText ?? "")).Distinct()
                .Take(limit).ToListAsync();
        }
    }
}