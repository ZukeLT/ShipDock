namespace ShipDock.Models
{
    public class LotStatistic
    {
        public Lot lot { get; set; }
        public IEnumerable<LotPosition> LotPositions { get; set; }
    }

    public class LotPosition
    {
        public int X_Cord { get; set; }
        public int Y_Cord { get; set; }
        public int Z_Cord { get; set; }
        public int LotPositionID { get; set; }
        public int LotID { get; set; }
        public int CargoID { get; set; }

        public int CargoWeight { get; set; }
    }
}
