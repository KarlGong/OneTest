using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OneTestApi.Models;

namespace OneTestApi.Data
{
    public class TestNodeConfiguration: IEntityTypeConfiguration<TestNode>
    {
        public void Configure(EntityTypeBuilder<TestNode> builder)
        {
            builder.Property(tn => tn.Name).IsRequired();

            builder.Property(tn => tn.InsertTime)
                .ValueGeneratedOnAdd();

            builder.Property(tn => tn.UpdateTime)
                .ValueGeneratedOnAddOrUpdate();
            
            builder.HasOne(tn => tn.Parent)
                .WithMany(tn => tn.Children)
                .HasForeignKey(tn => tn.ParentId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasQueryFilter(tn => !tn.IsDeleted);
        }
    }
}