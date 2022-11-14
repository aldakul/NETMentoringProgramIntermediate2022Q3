namespace Complex.LinqProvider.Entities
{
    public record Employee : BaseEntity
    {
        public int Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; set; }
        public int OfficeId { get; set; }
    }
}
