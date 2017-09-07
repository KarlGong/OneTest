using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OneTestApi.Models;

namespace OneTestApi.Data
{
    public class TestProjectConfiguration: IEntityTypeConfiguration<TestProject>
    {
        public void Configure(EntityTypeBuilder<TestProject> builder)
        {
            
        }
    }
}