namespace movies_api.Models
{
    public class Movie
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public double Rating { get; set; }
        public List<string> Genre { get; set; }
        public List<string> Stars { get; set; }
        public string Cover { get; set; }
        public string Imdb { get; set; }
    }

}
