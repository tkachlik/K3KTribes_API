using AutoMapper;
using dusicyon_midnight_tribes_backend.Models.Entities;
using dusicyon_midnight_tribes_backend.Models.Entities.DTOs;

namespace dusicyon_midnight_tribes_backend.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Player, PlayerDTO>()
                .ForMember(dto => dto.VerifiedAt, opt => opt.MapFrom(src => src.VerifiedAt.Value.ToString("yyyy.MM.dd. HH:mm:ss")));

            CreateMap<Player, PlayerCreatedDTO>();

            CreateMap<Kingdom, KingdomDTO>()
                .ForMember(dto => dto.World, opt => opt.MapFrom(src => src.World.Name))
                .ForMember(dto => dto.Owner, opt => opt.MapFrom(src => src.Player.UserName))
                .ForMember(dto => dto.OwnerId, opt => opt.MapFrom(src => src.PlayerId));

            CreateMap<Kingdom, MyKingdomDTO>()
                .ForMember(dto => dto.World, opt => opt.MapFrom(src => src.World.Name));

            CreateMap<Kingdom, KingdomStatsDTO>()
                .ForMember(dto => dto.World, opt => opt.MapFrom(src => src.World.Name))
                .ForMember(dto => dto.Owner, opt => opt.MapFrom(src => src.Player.UserName))
                .ForMember(dto => dto.OwnerId, opt => opt.MapFrom(src => src.PlayerId))
                .ForMember(dto => dto.GoldAmount, opt => opt.MapFrom
                    (src => src.Resources.FirstOrDefault(r => r.ResourceTypeId == 1).Amount))
                .ForMember(dto => dto.FoodAmount, opt => opt.MapFrom
                    (src => src.Resources.FirstOrDefault(r => r.ResourceTypeId == 2).Amount))
                .ForMember(dto => dto.TownhallLevel, opt => opt.MapFrom
                    (src => src.Buildings.FirstOrDefault
                        (b => b.BuildingType.Name == "Townhall").BuildingType.Level));

            CreateMap<World, WorldDTO>()
                .ForMember(dto => dto.KingdomCount, opt => opt.MapFrom(src => src.Kingdoms.Count()));
            //.ForMember(dto => dto.PlayerNames, opt => opt.MapFrom(src => src.PlayerWorlds.Select(p => p.Player.UserName)));

            CreateMap<ProductionOption, ProductionOptionDTO>()
                .ForMember(dto => dto.ResourceType, opt => opt.MapFrom(src => src.ResourceType.Name))
                .ForMember(dto => dto.AmountProduced, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dto => dto.ProductionTime, opt => opt.MapFrom(src => src.ProdTime))
                .ForMember(dto => dto.ProductionOptionId, opt => opt.MapFrom(src => src.Id));

            CreateMap<Building, ProductionOptionsAvailableOnCompletedBuildingsDTO>()
                .ForMember(dto => dto.BuildingType, opt => opt.MapFrom(src => src.BuildingType.Name))
                .ForMember(dto => dto.BuildingLevel, opt => opt.MapFrom(src => src.BuildingType.Level))
                .ForMember(dto => dto.BuildingId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dto => dto.ProductionOptions, opt => opt.MapFrom(src => src.BuildingType.ProductionOptions));

            CreateMap<Production, ProductionDTO>()
                .ForMember(dto => dto.ResourceType, opt => opt.MapFrom(src => src.ProductionOption.ResourceType.Name))
                .ForMember(dto => dto.AmountProduced, opt => opt.MapFrom(src => src.ProductionOption.Amount))
                .ForMember(dto => dto.IsReadyForCollection, opt => opt.MapFrom(src => src.CompletedAt < DateTime.Now))
                .ForMember(dto => dto.ReadyAt, opt => opt.MapFrom(src => src.CompletedAt.ToString("yyyy.MM.dd. HH:mm:ss")))
                .ForMember(dto => dto.ProductionId, opt => opt.MapFrom(src => src.Id));

            CreateMap<Production, ProductionCreatedDTO>()
                .ForMember(dto => dto.ResourceType, opt => opt.MapFrom(src => src.ProductionOption.ResourceType.Name))
                .ForMember(dto => dto.AmountProduced, opt => opt.MapFrom(src => src.ProductionOption.Amount))
                .ForMember(dto => dto.WillBeReadyAt, opt => opt.MapFrom(src => src.CompletedAt.ToString("yyyy.MM.dd. HH:mm:ss")))
                .ForMember(dto => dto.ProductionId, opt => opt.MapFrom(src => src.Id));

            CreateMap<Production, ProductionCollectedOrDeletedDTO>()
                .ForMember(dto => dto.ResourceType, opt => opt.MapFrom(src => src.ProductionOption.ResourceType.Name))
                .ForMember(dto => dto.AmountProduced, opt => opt.MapFrom(src => src.ProductionOption.Amount));

            CreateMap<Building, BuildingCreatedDTO>()
                .ForMember(dto => dto.BuildingType, opt => opt.MapFrom(src => src.BuildingType.Name))
                .ForMember(dto => dto.BuildingLevel, opt => opt.MapFrom(src => src.BuildingType.Level))
                .ForMember(dto => dto.BuildingId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dto => dto.BelongsToKingdom, opt => opt.MapFrom(src => src.Kingdom.Name))
                .ForMember(dto => dto.KingdomId, opt => opt.MapFrom(src => src.KingdomId));
        }
    }
}