namespace ShopGeneral.Services;

public class ProductServiceModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public int ManufacturerId { get; set; }
    public string ManufacturerName { get; set; }
    public string ImageUrl { get; set; }
    public int Price { get; set; }
    public int BasePrice { get; set; }
    public DateTime AddedUtc { get; set; }
}