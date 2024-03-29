﻿using dusicyon_midnight_tribes_backend.Models.APIResponses.Templates;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;

namespace dusicyon_midnight_tribes_backend.Models.APIResponses.Worlds;

public class CreateWorldResponse : IResponse
{
    public string Status { get; } = "Ok";
    public WorldDTO World { get; init; }

    public override bool Equals(object? obj)
    {
        var other = obj as CreateWorldResponse;

        if (other == null) return false;

        if (Status != other.Status || !World.Equals(other.World)) return false;

        return true;
    }

}
