namespace BookStoreApi.Models;

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int AuthorId { get; set; }
    public int CategoryId { get; set; }
    public DateTime PublishedDate { get; set; }
}
