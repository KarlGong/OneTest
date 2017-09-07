using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OneTestApi.Models;

namespace OneTestApi.Data
{
    public class TestCaseTagConfiguration: IEntityTypeConfiguration<TestCaseTag>
    {
        public void Configure(EntityTypeBuilder<TestCaseTag> builder)
        {
            builder.Property(tct => tct.Value).IsRequired();
            
            builder.Property(tct => tct.InsertTime)
                .ValueGeneratedOnAdd();
            
            builder.Property(tct => tct.UpdateTime)
                .ValueGeneratedOnAddOrUpdate();
            
            builder.HasOne(tct => tct.TestCase)
                .WithMany(tc => tc.Tags)
                .HasForeignKey(tct => tct.TestCaseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}