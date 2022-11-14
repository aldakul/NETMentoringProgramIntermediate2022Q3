namespace Complex.LinqProvider.Entities
{
    public record Office : BaseEntity
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string OfficeChief { get; init; }
        public int CountryId { get; init; }
    }
}
