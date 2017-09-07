using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OneTestApi.Models;

namespace OneTestApi.Data
{
    public class TestStepConfiguration: IEntityTypeConfiguration<TestStep>
    {
        public void Configure(EntityTypeBuilder<TestStep> builder)
        {
            builder.Property(ts => ts.InsertTime)
                .ValueGeneratedOnAdd();

            builder.Property(ts => ts.UpdateTime)
                .ValueGeneratedOnAddOrUpdate();
            
            builder.HasOne(ts => ts.TestCase)
                .WithMany(tc => tc.TestSteps)
                .HasForeignKey(ts => ts.TestCaseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}