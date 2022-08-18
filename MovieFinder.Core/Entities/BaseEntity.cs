using System.ComponentModel.DataAnnotations;
using MovieFinder.Core.Helpers;

namespace MovieFinder.Core.Entities;

public class BaseEntity
{
    public BaseEntity()
    {
        Id = DateTime.Now.Ticks.toBase36();
    }

    [Key]
    public string Id { get; set; }
}