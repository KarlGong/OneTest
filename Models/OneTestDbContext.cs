using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OneTestApi.Models
{
    public class OneTestDbContext : DbContext
    {
        public OneTestDbContext(DbContextOptions<OneTestDbContext> options) : base(options)
        {
        }

        public DbSet<TestNode> TestNodes { get; set; }
        public DbSet<TestProject> TestProjects { get; set; }
        public DbSet<TestSuite> TestSuites { get; set; }
        public DbSet<TestCase> TestCases { get; set; }
        public DbSet<TestStep> TestSteps { get; set; }
        public DbSet<TestCaseTag> TestCaseTags { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestNode>().HasOne(tn => tn.Parent).WithMany(tn => tn.Children)
                .HasForeignKey(tn => tn.ParentId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}