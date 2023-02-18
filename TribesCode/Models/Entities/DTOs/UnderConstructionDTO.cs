namespace dusicyon_midnight_tribes_backend.Models.Entities.DTOs;

public class UnderConstructionDTO
{
    public int BuildingId { get; set; }
    
    public string Name { get; set; }
    
    public string CompletionTime { get; set; }

    public UnderConstructionDTO(int buildingId, string name, DateTime completionTime)
    {
        BuildingId = buildingId;
        Name = name;
        CompletionTime = completionTime.ToString("yyyy.MM.dd. HH:mm:ss");

    }

}