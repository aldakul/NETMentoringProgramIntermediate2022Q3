namespace Complex.LinqProvider.Entities
{
    public record Country : BaseEntity
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }
}
