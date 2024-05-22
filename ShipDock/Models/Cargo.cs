using ShipDock.Helper;
using System.Reflection;

namespace ShipDock.Models
{
    public class Cargo
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public double Weight { get; set; }
        public DateTime AcceptanceDate { get; set; }
        public int TracktorID { get; set; }
        public int UserID { get; set; }
        public int LotPositionID { get; set; }
        public string? State { get; set; }

        public bool Insert() {
            if (ID == 0)
            {
                return DataSource.UpdateDataSQL($"INSERT INTO [dbo].[Cargo] ([Title],[Code],[Weight],[AcceptanceDate],[TracktorID],[UserID],[LotPositionID]) " +
                    $"VALUES ('{Name}','{Code}',{Weight},'{AcceptanceDate}',{0},{0},{0})");
            }
            return false;
        }
    }

}