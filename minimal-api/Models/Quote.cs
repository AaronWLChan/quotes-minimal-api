namespace minimal_api.Models
{
    public class Quote
    {
        public int Id { get; set; }
        public string? Saying { get; set; }

        public Character? SaidBy { get; set; }

        public TVShow? TVShow { get; set; }



    }
}
