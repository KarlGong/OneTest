using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OneTestApi.Models;

namespace OneTestApi.Data
{
    public class TestSuiteConfiguration: IEntityTypeConfiguration<TestSuite>
    {
        public void Configure(EntityTypeBuilder<TestSuite> builder)
        {
            
        }
    }
}