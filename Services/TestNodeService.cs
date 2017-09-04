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

        Task<List<TestNode>> GetAncestorsAsync(int id);

        Task<List<TestNode>> GetDescendantsAsync(int id);

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

        public async Task<List<TestNode>> GetAncestorsAsync(int id)
        {
            var parent = await GetParentAsync(id);
            var ancestors = new List<TestNode>();

            while (parent != null)
            {
                ancestors.Add(parent);
                parent = await GetParentAsync(parent.Id);
            }

            return ancestors;
        }

        public async Task<List<TestNode>> GetDescendantsAsync(int id)
        {
            var descendants = new List<TestNode>();
            
            foreach (var child in await GetChildrenAsync(id))
            {
                descendants.Add(child);

                descendants.AddRange(await GetDescendantsAsync(child.Id));    
            }

            return descendants;
        }

        public async Task<List<TestNode>> GetChildrenAsync(int id)
        {
            return await _context.TestNodes.Where(tn => tn.ParentId == id).OrderBy(tn => tn.Position).ToListAsync();
        }

        /// <summary>
        /// Move to parent and position.
        /// 
        /// ToParentId null means moving to root.
        /// Negative ToPosition means appending to bottom of parent.
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

                var previouseParentId = previousTestNode.ParentId.GetValueOrDefault();
                var toParentId = ps.ToParentId.GetValueOrDefault();

                previousTestNode.Position = ps.ToPosition;
                previousTestNode.ParentId = ps.ToParentId;
                
                // calc count
                var previousAncestors = new List<TestNode>();
                previousAncestors.Add(await GetAsync(previouseParentId));
                previousAncestors.AddRange(await GetAncestorsAsync(previouseParentId));
                
                var ancestors = new List<TestNode>();
                ancestors.Add(await GetAsync(toParentId));
                ancestors.AddRange(await GetAncestorsAsync(toParentId));
                
                foreach (var previousAncestor in previousAncestors.Except(ancestors))
                {
                    previousAncestor.Count -= previousTestNode.Count;
                }

                foreach (var ancestor in ancestors.Except(previousAncestors))
                {
                    ancestor.Count += previousTestNode.Count;
                }
            }
            
            await _context.SaveChangesAsync();
        }
    }
}