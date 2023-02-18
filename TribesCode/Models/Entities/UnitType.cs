using System.ComponentModel.DataAnnotations.Schema;

namespace dusicyon_midnight_tribes_backend.Models.Entities
{
    public class UnitType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        private int attack;
        public int Attack
        {
            get => attack;
            set => attack = value < 0 ? 0 : value;
        }
        
        private int defense;
        public int Defense
        {
            get => defense;
            set => defense = value < 0 ? 0 : value;
        }
        private int hpTotal;
        public int HPTotal
        {
            get => hpTotal;
            set => hpTotal = value < 0 ? 0 : value;
        }

        [ForeignKey("ProductionOption")]
        public int ProductionOptionId { get; set; }
        public ProductionOption ProductionOption { get; set; }

        [ForeignKey("UnitCost")]
        public int UnitCostId { get; set; }
        public UnitCost UnitCost { get; set; }

        public UnitType() { }

        public UnitType(string name, int attack, int defense, int hpTotal)
        {
            Name = name;
            Attack = attack;
            Defense = defense;
            HPTotal = hpTotal;
        }
    }
}