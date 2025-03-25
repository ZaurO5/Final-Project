namespace Core.Entities;
public class Color : BaseEntity
{
    public string Name { get; set; }
    public string HexCode { get; set; }
    public ICollection<ProductColors> ProductColors { get; set; }
}
