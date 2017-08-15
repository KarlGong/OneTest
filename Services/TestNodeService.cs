using System;
using System.Collections.Generic;
using System.Linq;
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
        TestNode Get(int id);

        TestNode GetParent(int id);

        List<TestNode> GetChildren(int id);

        void Move(MoveTestNodeParams ps);
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
            return _context.TestNodes.Include(ts => ts.Children).Single(ts => ts.Id == id).Children
                .OrderBy(child => child.Position).ToList();
        }
        
        /// <summary>
        /// Move to parent and position.
        /// 
        /// ToParentId null means moving to root.
        /// Negative ToPosition means appending to parent.
        /// </summary>
        /// <param name="ps"></param>
        public void Move(MoveTestNodeParams ps)
        {
            var sibingsCount = _context.TestNodes.Count(tn => tn.ParentId == ps.ToParentId);
            ps.ToPosition = ps.ToPosition <= -1 ? sibingsCount : Math.Min(ps.ToPosition, sibingsCount);
            
            var previousTestNode = Get(ps.Id);

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
            _context.SaveChanges();
        }
    }
}