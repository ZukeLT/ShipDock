using ShipDock.Helper;
using System.Reflection;

namespace ShipDock.Models
{
    public class Lot
    {
        public int ID { get; set; }
        public string? Code { get; set; }
        public int X_Dim { get; set; }
        public int Y_Dim { get; set; }
        public int Z_Dim { get; set; }
        public bool Insert()
        {
            if (ID == 0)
            {
                return DataSource.UpdateDataSQL($"INSERT INTO [dbo].[Lot] ([Code],[X_Dim],[Y_Dim],[Z_Dim]) " +
                    $"VALUES ('{Code}',{X_Dim},{Y_Dim},{Z_Dim})");
            }
            return false;
        }
    }
}
