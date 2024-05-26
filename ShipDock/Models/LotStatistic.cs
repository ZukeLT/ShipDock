using ShipDock.Helper;
using System.Data;

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

        public static List<LotPosition> SelectAll()
        {
            DataView dataView = DataSource.ExecuteSelectSQL("SELECT * FROM LotPosition");
            return ConvertDataViewToList(dataView);
        }

        public int Insert()
        {
            string query = $"INSERT INTO LotPosition (LotID, X_Cord, Y_Cord, Z_Cord) VALUES ({LotID}, {X_Cord}, {Y_Cord}, {Z_Cord})";
            return DataSource.InsertSQL(query);
        }

        public bool Update()
        {
            if (LotPositionID > 0)
            {
                string query = $"UPDATE LotPosition SET LotID = {LotID}, X_Cord = {X_Cord}, Y_Cord = {Y_Cord}, Z_Cord = {Z_Cord} WHERE LotPositionID = {LotPositionID}";
                return DataSource.UpdateDataSQL(query);
            }
            return false;
        }

        private static List<LotPosition> ConvertDataViewToList(DataView dataView)
        {
            List<LotPosition> lotPositions = new List<LotPosition>();

            foreach (DataRowView rowView in dataView)
            {
                DataRow row = rowView.Row;
                LotPosition lotPosition = new LotPosition
                {
                    LotPositionID = row.Field<int>("LotPositionID"),
                    LotID = row.Field<int>("LotID"),
                    X_Cord = row.Field<int>("X_Cord"),
                    Y_Cord = row.Field<int>("Y_Cord"),
                    Z_Cord = row.Field<int>("Z_Cord")
                };

                lotPositions.Add(lotPosition);
            }

            return lotPositions;
        }
    }
}
