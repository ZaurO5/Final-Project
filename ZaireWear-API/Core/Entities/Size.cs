namespace Core.Entities;
public class Size : BaseEntity
{
    public string Name { get; set; }
    public ICollection<ProductSizes> ProductSizes { get; set; }
}
