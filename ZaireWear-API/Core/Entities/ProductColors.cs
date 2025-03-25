namespace Core.Entities;
public class ProductColors
{
    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int ColorId { get; set; }
    public Color Color { get; set; }
}
