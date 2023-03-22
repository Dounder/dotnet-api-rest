using Core.Interfaces.Common;

namespace Core.Entities.Common;

public class BaseEntity : IBaseEntity
{
    public Guid Id { get; set; } = new Guid();

    public int Iid { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public DateTime? DeletedAt { get; set; }
}