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

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentException("Name is required");
        if (string.IsNullOrWhiteSpace(Address))
            throw new ArgumentException("Address is required");
        if (string.IsNullOrWhiteSpace(Phone))
            throw new ArgumentException("Phone is required");
        if (string.IsNullOrWhiteSpace(Email))
            throw new ArgumentException("Email is required");
    }
}