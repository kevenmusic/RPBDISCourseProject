namespace MarriageAgency.Models;

public partial class AdditionalService
{
    public int AdditionalServiceId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
