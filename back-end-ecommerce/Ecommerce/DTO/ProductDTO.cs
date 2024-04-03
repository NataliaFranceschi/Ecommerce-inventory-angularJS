public class ProductRequest
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public class ProductDTO : ProductRequest
{
    public Guid Id { get; set; }

}