namespace dusicyon_midnight_tribes_backend.Models.Entities.DTOs;

public class UpgradeOptionsDTO
{
    public int BuildingId { get; set; }
    
    public string Name { get; set; }

    public int CurrentLevel { get; set; }
    
    public int HighestAvailableLevel { get; set; }
    
    public int NextLevelPrice { get; set; }

    public UpgradeOptionsDTO(int buildingId, string name, int currentLevel, int availableLevel, int nextLevelPrice)
    {
        BuildingId = buildingId;
        Name = name;
        CurrentLevel = currentLevel;
        HighestAvailableLevel = availableLevel;
        NextLevelPrice = nextLevelPrice;
    }

}