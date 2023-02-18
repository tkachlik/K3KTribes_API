namespace dusicyon_midnight_tribes_backend.Models.Entities
{
    public class Unit
    {
        public int Id { get; set; }
        public int UnitTypeId { get; set; }
        public UnitType UnitType { get; set; }
        public int ArmyId { get; set; }
        
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
        
        private int hpCurrent;
        public int HPCurrent
        {
            get => hpCurrent;
            set
            {
                if (value > HPTotal)
                {
                    hpCurrent = HPTotal;
                }
                else if (value < 0)
                {
                    hpCurrent = 0;
                }
                else
                {
                    hpCurrent = value;
                }                   
            }
            //this one is longer because there are 3 possible outcomes instead of the usual 2
        }

        public Unit()
        {
        }

        public Unit(int unitTypeId, int armyId, int attack, int defense, int hpTotal)
        {
            UnitTypeId = unitTypeId;
            ArmyId = armyId;
            Attack = attack;
            Defense = defense;
            HPTotal = hpTotal;
            HPCurrent = hpTotal; // it's setup this way so that the unit start with full hp
        }
    }
}
