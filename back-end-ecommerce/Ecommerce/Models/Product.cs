using System.ComponentModel.DataAnnotations;

public class Product: BaseModel
{
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Image {  get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }

}
