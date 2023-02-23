namespace dusicyon_midnight_tribes_backend.Models.Entities.DTOs;

public class ConstructionOptionsDTO
{
    // has to be hardcoded because different buildings have different limits
    public string Name { get; set; }

    public int Limit { get; set; }

    public int Available { get; set; }

    public ConstructionOptionsDTO(string name, int limit, int available)
    {
        Name = name;
        Limit = limit;
        Available = available;
    }
}