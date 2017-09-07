using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OneTestApi.Models;

namespace OneTestApi.Data
{
    public class TestCaseConfiguration: IEntityTypeConfiguration<TestCase>
    {
        public void Configure(EntityTypeBuilder<TestCase> builder)
        {
            
        }
    }
}