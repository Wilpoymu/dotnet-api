using System.ComponentModel.DataAnnotations;

namespace softsolutions.Models;

public class Category
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } 
    public List<Product>? Products { get; set; }
    
}