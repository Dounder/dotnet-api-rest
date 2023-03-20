namespace Core.Interfaces.Common;

public interface IBaseEntity
{
    public Guid Id { get; set; }
    public int IID { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}