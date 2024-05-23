using Microsoft.AspNetCore.Mvc;
using ShipDock.Helper;
using ShipDock.Models;
using System.Data;

namespace ShipDock.Controllers.LotInformation
{
    public class LotInformationController : Controller
    {
        public IActionResult LotInformation()
        {
            List<Lot> lotList = new List<Lot>();
            DataView dw = (DataView)DataSource.ExecuteSelectSQL("SELECT * FROM Lot");

            if(dw != null)
            {
                foreach(DataRow row in dw.Table.Rows)
                {
                    Lot lot = new Lot();
                    lot.ID = int.Parse(row["LotID"].ToString());
                    lot.Code = row["Code"].ToString();
                    lot.X_Dim = int.Parse(row["X_Dim"].ToString());
                    lot.Y_Dim = int.Parse(row["Y_Dim"].ToString());
                    lot.Z_Dim = int.Parse(row["Z_Dim"].ToString());
                    lotList.Add(lot);
                }
            }
            return View(lotList);
        }

        public IActionResult ViewLotStatistic(int id)
        {

            DataView dw = (DataView)DataSource.ExecuteSelectSQL($"SELECT LotID, Code, X_Dim, Y_Dim, Z_Dim FROM Lot WHERE LotID = '{id}'");
            LotStatistic statistic = new LotStatistic();
            Lot lot = new Lot();

            if(dw != null)
            {
                DataRow row = dw.Table.Rows[0];
                lot.ID = int.Parse(row["LotID"].ToString());
                lot.Code = row["Code"].ToString();
                lot.X_Dim = int.Parse(row["X_Dim"].ToString());
                lot.Y_Dim = int.Parse(row["Y_Dim"].ToString());
                lot.Z_Dim = int.Parse(row["Z_Dim"].ToString());
                statistic.lot = lot;
            }


            List<LotPosition> positions = new List<LotPosition>();
            DataView dw2 = (DataView)DataSource.ExecuteSelectSQL($"SELECT LotPosition.X_Cord, LotPosition.Y_Cord, LotPosition.Z_Cord, LotPosition.LotPositionID, LotPosition.LotID, Cargo.CargoID, Cargo.Weight FROM LotPosition " +
                $"JOIN Cargo ON Cargo.LotPositionID = LotPosition.LotPositionID WHERE LotPosition.LotID = {id}");
            
            if(dw2 != null)
            {
                foreach(DataRow row in dw2.Table.Rows)
                {
                    LotPosition position = new LotPosition();
                    position.X_Cord = int.Parse(row["X_Cord"].ToString());
                    position.Y_Cord = int.Parse(row["Y_Cord"].ToString());
                    position.Z_Cord = int.Parse(row["Z_Cord"].ToString());
                    position.LotPositionID = int.Parse(row["LotPositionID"].ToString());
                    position.LotID = int.Parse(row["LotID"].ToString());
                    position.CargoID = int.Parse(row["CargoID"].ToString());
                    position.CargoWeight = int.Parse(row["Weight"].ToString());
                    positions.Add(position);
                }
            }
            statistic.LotPositions = positions;


            return View(statistic);

        }
    }



}
