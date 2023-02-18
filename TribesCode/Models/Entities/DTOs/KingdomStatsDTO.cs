namespace dusicyon_midnight_tribes_backend.Models.Entities.DTOs;

public class KingdomStatsDTO 
{
    public string Name { get; set; }
    public int Id { get; set; }
    public string World { get; set; }
    public int WorldId { get; set; }
    public string Owner { get; set; }
    public int OwnerId { get; set; }
    public int TownhallLevel { get; set; }
    public int MaxStorage { get; set; }
    public int GoldAmount { get; set; }
    public int FoodAmount { get; set; }
    
    public override bool Equals(object? obj)
    {
        var other = obj as KingdomStatsDTO;

        if (other == null) return false;

        if (Id != other.Id || Name != other.Name || World != other.World || WorldId != other.WorldId || Owner != other.Owner || OwnerId != other.OwnerId || TownhallLevel != other.TownhallLevel|| MaxStorage != other.MaxStorage || FoodAmount != other.FoodAmount || GoldAmount != other.GoldAmount) return false;

        else return true;
    }
}