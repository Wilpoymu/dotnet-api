using System.ComponentModel.DataAnnotations;

namespace softsolutions.Models;

public class Product
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = default!;
    [Required]
    public decimal Price { get; set; }
    public string Description { get; set; } = default!;
    public int CategoryId { get; set; }
    public Category Categories { get; set; } = default!;
    public List<Provider>? Providers { get; set; }
}