public class Movie{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<MovieCategory> MovieCategories { get; set; }
}