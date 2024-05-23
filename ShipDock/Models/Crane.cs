using ShipDock.Helper;

namespace ShipDock.Models
{
    public class Crane
    {
        public int CraneID { get; set; }
        public string CraneNr { get; set; }
        public double? MaxWeight { get; set; }
        public int DockID { get; set; }
        public int UserID { get; set; }

        public bool Insert()
        {
            if (CraneID == 0)
            {
                return DataSource.UpdateDataSQL($"INSERT INTO [dbo].[Crane] ([MaxWeight], [CraneNr], [DockID], [UserID]) " +
                    $"VALUES ({MaxWeight}, '{CraneNr}', '{DockID}', {UserID})");
            }
            return false;
        }

        public bool Update()
        {
            if (CraneID != 0)
            {
                return DataSource.UpdateDataSQL($"UPDATE [dbo].[Crane] SET " +
                    $"[MaxWeight] = {MaxWeight}, " +
                    $"[CraneNr] = '{CraneNr}', " +
                    $"[DockID] = '{DockID}', " +
                    $"[UserID] = {UserID} " +
                    $"WHERE [CraneID] = {CraneID}");
            }
            return false;
        }

        public bool Delete()
        {
            if (CraneID != 0)
            {
                return DataSource.UpdateDataSQL($"DELETE FROM [dbo].[Crane] WHERE [CraneID] = {CraneID}");
            }
            return false;
        }
    }
}
