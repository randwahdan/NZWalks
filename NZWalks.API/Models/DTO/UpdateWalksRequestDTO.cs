namespace NZWalks.API.Models.DTO
{
    public class UpdateWalksRequestDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }

        public string? WalksImageUrl { get; set; }
        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }
    }
}
