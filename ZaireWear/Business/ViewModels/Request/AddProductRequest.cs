namespace Business.ViewModels.Request;
public class AddProductRequest
{
    public int ProductId { get; set; }
    public string Color { get; set; }
    public string Size { get; set; }
    public int Quantity { get; set; }
}