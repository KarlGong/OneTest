namespace OneTestApi.Controllers.DTOs
{
    public class TestStepDto
    {
        public int Id { get; set; }
        
        public string Action { get; set; }
        
        public string ExpectedResult { get; set; }
    }
}