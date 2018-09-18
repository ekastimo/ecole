namespace Core.Models
{
    public class SearchBase
    {
        public string Query { get; set; }
        public int Skip { get; set; } = 0;
        public int Limit { get; set; } = 50;
    }
}