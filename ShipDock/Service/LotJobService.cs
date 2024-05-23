using Microsoft.AspNetCore.Mvc;
using ShipDock.Helper;
using ShipDock.Models;
using System.Data;

namespace ShipDock.Service
{
    public class LotJobService
    {
        public void CheckTracktorStatuses()
        {
            var tracktors = DataSource.ExecuteSelectSQL("SELECT * FROM Tracktor WHERE Status = 'Waiting'");

            foreach (DataRow row in tracktors.Table.Rows)
            {
                int tracktorId = (int)row["TracktorID"];
                var cranes = DataSource.ExecuteSelectSQL("SELECT * FROM Crane WHERE Status = 'Available'");

                if (cranes.Table.Rows.Count > 0)
                {
                    DataRow crane = cranes.Table.Rows[0];
                    int craneId = (int)crane["CraneID"];

                    DataSource.UpdateDataSQL($"UPDATE Tracktor SET Status = 'Assigned', AssignedCraneID = {craneId} WHERE TracktorID = {tracktorId}");
                    DataSource.UpdateDataSQL($"UPDATE Crane SET Status = 'Busy' WHERE CraneID = {craneId}");
                }
                else
                {
                    // Handle no available cranes
                }
            }
        }
        public void RecalculateShipCargoPositions()
        {
        }
        public void ConfirmCargoLoading(Cargo cargo)
        {
        }
    }
}
