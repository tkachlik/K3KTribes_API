namespace dusicyon_midnight_tribes_backend.Models.Entities
{
    public class Army
    {
        public int Id { get; set; }
        
        public int KingdomId { get; set; }     
        public Kingdom Kingdom { get; set; }
        
        public List<Unit> Units { get; set; }
        public ArmyType ArmyType { get; set; }

        public Army()
        {
        }

        public Army(int kingdomId, string type)
        {
            this.KingdomId = kingdomId;
            this.ArmyType = (ArmyType)Enum.Parse(typeof(ArmyType),type,true); // has to be parsed from string to enum (boolean value true makes it ignore case)
            this.Units = new List<Unit>();
        }

    }
   
    
}

