namespace dusicyon_midnight_tribes_backend.Models.Entities.DTOs;

public class KingdomDTO 
{
    public string Name { get; set; }
    public int Id { get; set; }
    public string World { get; set; }
    public int WorldId { get; set; }
    public string Owner { get; set; }
    public int OwnerId { get; set; }
    
    public override bool Equals(object? obj)
    {
        var other = obj as KingdomDTO;

        if (other == null) return false;

        if (Id != other.Id || Name != other.Name || World != other.World || WorldId != other.WorldId || Owner != other.Owner || OwnerId != other.OwnerId) return false;

        else return true;
    }
}