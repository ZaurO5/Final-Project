namespace Business.ViewModels.Basket;

public class BasketUpdateVM
{
    public int BasketId { get; set; }
    public int ProductId { get; set; }
    public int ColorId { get; set; }
    public int SizeId { get; set; }
    public int Count { get; set; }
}