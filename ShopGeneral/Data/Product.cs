using System.ComponentModel.DataAnnotations;

namespace ShopGeneral.Data;

public class Product
{
    public int Id { get; set; }
    [MaxLength(50)] public string Name { get; set; }
    public Category Category { get; set; }
    public Manufacturer Manufacturer { get; set; }
    public int BasePrice { get; set; }
    public DateTime AddedUtc { get; set; }
    [MaxLength(100)] public string ImageUrl { get; set; }
}