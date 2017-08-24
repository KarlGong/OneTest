using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OneTestApi.Models;

namespace OneTestApi.Services
{
    public class MoveTestNodeParams
    {
        public int Id { get; set; }
        
        public int? ToParentId { get; set; }
        
        public int ToPosition { get; set; }
    }
    
    public interface ITestNodeService
    {
        Task<TestNode> GetAsync(int id);

        Task<TestNode> GetParentAsync(int id);

        Task<List<TestNode>> GetChildrenAsync(int id);

        Task MoveAsync(MoveTestNodeParams ps);
    }

    public class TestNodeService : ITestNodeService
    {
        private readonly OneTestDbContext _context;

        public TestNodeService(OneTestDbContext context)
        {
            _context = context;
        }

        public async Task<TestNode> GetAsync(int id)
        {
            return await _context.TestNodes.SingleAsync(tn => tn.Id == id);
        }

        public async Task<TestNode> GetParentAsync(int id)
        {
            return (await _context.TestNodes.Include(tn => tn.Parent).SingleAsync(tn => tn.Id == id)).Parent;
        }

        public async Task<List<TestNode>> GetChildrenAsync(int id)
        {
            return await _context.TestNodes.Where(tn => tn.ParentId == id).OrderBy(tn => tn.Position).ToListAsync();
        }
        
        /// <summary>
        /// Move to parent and position.
        /// 
        /// ToParentId null means moving to root.
        /// Negative ToPosition means appending to parent.
        /// </summary>
        /// <param name="ps"></param>
        public async Task MoveAsync(MoveTestNodeParams ps)
        {
            var sibingsCount = await _context.TestNodes.CountAsync(tn => tn.ParentId == ps.ToParentId);
            ps.ToPosition = ps.ToPosition <= -1 ? sibingsCount : Math.Min(ps.ToPosition, sibingsCount);
            
            var previousTestNode = await GetAsync(ps.Id);

            if (previousTestNode.ParentId == ps.ToParentId)
            {
                if (previousTestNode.Position > ps.ToPosition)
                {
                    foreach (var node in _context.TestNodes.Where(tn =>
                        tn.ParentId == ps.ToParentId && tn.Position >= ps.ToPosition &&
                        tn.Position < previousTestNode.Position))
                    {
                        node.Position++;
                    }
                    
                    previousTestNode.Position = ps.ToPosition;
                }
                else if (previousTestNode.Position < ps.ToPosition)
                {
                    foreach (var node in _context.TestNodes.Where(tn =>
                        tn.ParentId == ps.ToParentId && tn.Position < ps.ToPosition &&
                        tn.Position > previousTestNode.Position))
                    {
                        node.Position--;
                    }
                    
                    previousTestNode.Position = ps.ToPosition - 1;
                }
            }
            else
            {
                foreach (var node in _context.TestNodes.Where(tn =>
                    tn.ParentId == previousTestNode.ParentId && tn.Position > previousTestNode.Position))
                {
                    node.Position--;
                }

                foreach (var node in _context.TestNodes.Where(tn =>
                    tn.ParentId == ps.ToParentId && tn.Position >= ps.ToPosition))
                {
                    node.Position++;
                }
                
                previousTestNode.Position = ps.ToPosition;
            }
            
            previousTestNode.ParentId = ps.ToParentId;
            await _context.SaveChangesAsync();
        }
    }
}