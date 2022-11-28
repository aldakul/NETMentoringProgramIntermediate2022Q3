namespace Categories.Domain;

public class Item
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid CategoryId { get; set; }
    public virtual Category Category { get; set; }
}
