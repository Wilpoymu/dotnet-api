using System.ComponentModel.DataAnnotations;

namespace softsolutions.Models;

public class Provider
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    [Required]
    public string Email { get; set; }
    public List<Product>? Products { get; set; }
}