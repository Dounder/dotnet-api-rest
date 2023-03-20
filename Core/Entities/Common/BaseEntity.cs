using System.ComponentModel.DataAnnotations.Schema;
using Core.Interfaces.Common;

namespace Core.Entities.Common;

public class BaseEntity : IBaseEntity
{
    public Guid Id { get; set; } = new Guid();

    public int IID { get; set; }

    [Column(TypeName = "date")] public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column(TypeName = "date")] public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [Column(TypeName = "date")] public DateTime? DeletedAt { get; set; }
}