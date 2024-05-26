using ShipDock.Helper;
using System.Data;
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
        public static List<Lot> ConvertDataViewToList(DataView dataView)
        {
            List<Lot> lots = new List<Lot>();
            if (dataView != null)
            {
                foreach (DataRow row in dataView.Table.Rows)
                {
                    Lot lot = new Lot();
                    lot.ID = int.Parse(row["LotID"].ToString());
                    lot.Code = row["Code"].ToString();
                    lot.X_Dim = int.Parse(row["X_Dim"].ToString());
                    lot.Y_Dim = int.Parse(row["Y_Dim"].ToString());
                    lot.Z_Dim = int.Parse(row["Z_Dim"].ToString());
                    lots.Add(lot);
                }
            }

            return lots;
        }
    }
}
