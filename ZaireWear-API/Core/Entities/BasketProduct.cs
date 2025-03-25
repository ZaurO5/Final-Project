namespace Core.Entities;
public class BasketProduct
{
    public int Id { get; set; }
    public int BasketId { get; set; }
    public int ProductId { get; set; }
    public int ColorId { get; set; }
    public int SizeId { get; set; }
    public int Count { get; set; }

    public Basket Basket { get; set; }
    public Product Product { get; set; }
    public Color Color { get; set; }
    public Size Size { get; set; }
}
