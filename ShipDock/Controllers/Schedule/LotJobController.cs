using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.DependencyResolver;
using ShipDock.Helper;
using ShipDock.Helpers;
using ShipDock.Models;
using ShipDock.Models.agreg;
using System.Collections.Generic;
using System.Data;
using System.Text;
using static ShipDock.Helpers.Enums;

namespace ShipDock.Controllers.Schedule
{
  
    public class LotJobController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult WorkerJobs()
        {
            return View("../Schedule/WorkerJobs");
        }
        public IActionResult Recalculate()
        {
            RecalculateShipCargoPositions();
            if (SelectCargoLoad().Table.Rows.Count > 0)
                CheckTracktorStatuses();

            return View("../Schedule/WorkerJobs");
        }
        public IActionResult AtvykoKroviniai()
        {
            DataSource.UpdateDataSQL(
               $"UPDATE Cargo " +
               $"SET [State] = '{Enums.CargoStates.Proccesing}' " +
               $"WHERE [State] = '{Enums.CargoStates.New}'");

            return View("../Schedule/WorkerJobs");
        }
        public IActionResult ListOfTracktors()
        {
            DataView tracktors = DataSource.ExecuteSelectSQL(
              $"SELECT * FROM Tracktor"
              );

            List<Tracktor> tracktorList = new List<Tracktor>();
            foreach (DataRow row in tracktors.Table.Rows)
            {
                Tracktor tracktor = new Tracktor();
                tracktor.TracktorID = (int)row["TracktorID"];
                tracktor.CargoID = (row["CargoID"] != DBNull.Value ?  (int)row["CargoID"] : null);
                tracktor.CraneID = (row["CraneID"] != DBNull.Value ? (int)row["CraneID"] : null);
                tracktor.Status = row["Status"].ToString();
                tracktor.Model = row["Model"].ToString();
                tracktorList.Add(tracktor);
            }

            return View("../Schedule/TracktorList", tracktorList);
        }
  

        #region LotJobController Methods
        public void CheckTracktorStatuses()
        {
            var tracktors = DataSource.ExecuteSelectSQL(
                $"SELECT * FROM Tracktor WHERE [Status] = '{TracktorStates.Waiting}' AND CargoID IS NULL AND CraneID IS NULL"
                );

            foreach (DataRow row in tracktors.Table.Rows)
            {
                int tracktorId = (int)row["TracktorID"];
                var cranes = DataSource.ExecuteSelectSQL(
                $"SELECT * FROM Crane " +
                $"RIGHT JOIN [User] ON [User].UserID = Crane.UserID " +
                $"WHERE [User].UserRole = 'Pakrovimo Darbuotojas' " +
                $"AND NOT EXISTS (SELECT * FROM Tracktor WHERE CraneID = Crane.CraneID)"); //Reikiaa parinkti tokius kurie dar nera tarp tracktor objecto

                if (cranes != null && cranes.Table.Rows.Count > 0)
                {
                    DataRow cranedata = cranes.Table.Rows[0];
                    Crane crane = new Crane
                    {
                        CraneID = (int)cranedata["CraneID"],
                        CraneNr = cranedata["CraneNr"].ToString(),
                        DockID = (int)cranedata["DockID"],
                        MaxWeight = (int)cranedata["DockID"],
                        UserID = (int)cranedata["CraneID"]
                    };

                    SendUnesignedDockCraneCoordinates(tracktorId, crane);
                }
                else
                    Console.WriteLine("No available cranes!!!!!!!!");
                
            }
        }
        public IActionResult ConfirmCargoLoading(string userJSON, string tracktorJSON, string cargoJSON)
        {
            User user = JsonConvert.DeserializeObject<User>(userJSON);
            Tracktor tracktor = JsonConvert.DeserializeObject<Tracktor>(tracktorJSON);
            Cargo cargo = JsonConvert.DeserializeObject<Cargo>(cargoJSON);

            var cranes = DataSource.ExecuteSelectSQL(
                $"SELECT * FROM Crane " +
                $"RIGHT JOIN [User] ON [User].UserID = Crane.UserID " +
                $"WHERE [User].UserRole = '{"Iškrovimo Darbuotojas"}'");

            if (cranes != null && cranes.Table.Rows.Count > 0)
            {
                DataRow cranedata = cranes.Table.Rows[0];
                Crane crane = new Crane
                {
                    CraneID = (int)cranedata["CraneID"],
                    CraneNr = cranedata["CraneNr"].ToString(),
                    DockID = (int)cranedata["DockID"],
                    MaxWeight = (int)cranedata["DockID"],
                    UserID = (int)cranedata["CraneID"]
                };

                CargoCrane cargoCrane = new CargoCrane()
                {
                    cargo = cargo,
                    crane = crane
                };



                SendCoordinatesWithLoad(tracktor.TracktorID, cargoCrane);

                DataSource.UpdateDataSQL(
                  $"UPDATE Cargo " +
                  $"SET [State] = '{Enums.CargoStates.Delivered}' " +
                  $"WHERE CargoID = {cargo.ID}");
            }



            Console.WriteLine("LoadedCargo action");
            return View("../Schedule/WorkerJobs");
        }
        public IActionResult ConfirmCargoUnloading(string userJSON, string tracktorJSON, string cargoJSON)
        {
            User user = JsonConvert.DeserializeObject<User>(userJSON);
            Tracktor tracktor = JsonConvert.DeserializeObject<Tracktor>(tracktorJSON);
            Cargo cargo = JsonConvert.DeserializeObject<Cargo>(cargoJSON);

            UpdateCargoLoadStatus(tracktor.TracktorID,cargo.ID);


            if(SelectCargoLoad().Table.Rows.Count>0)
                CheckTracktorStatuses();
            else
                SendTracktorHome(tracktor.TracktorID);
            

            return View("../Schedule/WorkerJobs");
        }
        private void UpdateCargoLoadStatus(int tracktorID, int cargoID)
        {
             DataSource.UpdateDataSQL(
              $"UPDATE Cargo " +
              $"SET [State] = '{Enums.CargoStates.Prepared}' " +
              $"WHERE CargoID = {cargoID}");

            #region ForstateChange
            DataSource.UpdateDataSQL(
              $"UPDATE Tracktor " +
              $"SET [CraneID] = NULL, " +
              $"[CargoID] = NULL " +
              $"WHERE TracktorID = {tracktorID}");
            #endregion
        }
        public IActionResult SendTracktorHome(string userJSON, string tracktorJSON)
        {
            User user = JsonConvert.DeserializeObject<User>(userJSON);
            Tracktor tracktor = JsonConvert.DeserializeObject<Tracktor>(tracktorJSON);

            SendTracktorHome(tracktor.TracktorID);

            return View("../Schedule/WorkerJobs");
        }
        private DataView SelectCargoLoad()
        {
            return DataSource.ExecuteSelectSQL(
              $"SELECT * FROM Cargo WHERE [State] = '{CargoStates.Proccesing}'"
              );

        }
        public void RecalculateShipCargoPositions()
        {
            List<Cargo> CLP = ToCargoList(DataSource.ExecuteSelectSQL($"SELECT * FROM Cargo WHERE [State] = '{Enums.CargoStates.Proccesing}'"));
            List<Lot> lotList = Lot.ConvertDataViewToList(DataSource.ExecuteSelectSQL($"SELECT * FROM Lot"));
            List<LotPosition> lotPosList = LotPosition.SelectAll();

            foreach(Cargo cargo in CLP)
            {
                Lot lot = lotList.FirstOrDefault();
                if(lot != null)
                {
                    List<LotPosition> positions = lotPosList.Where(n => n.LotID == lot.ID).ToList();
                    HashSet<string> existingPositions = new HashSet<string>(positions.Select(p => $"{p.X_Cord},{p.Y_Cord},{p.Z_Cord}"));
                    bool breakBool = false;
                    for (int z = 0; z < lot.Z_Dim; z++)
                    {
                        for (int y = 0; y < lot.Y_Dim; y++)
                        {
                            for (int x = 0; x < lot.X_Dim; x++)
                            {
                                string posKey = $"{x},{y},{z}";
                                if (!existingPositions.Contains(posKey))
                                {
                                    LotPosition newPosition = new LotPosition
                                    {
                                        LotID = lot.ID,
                                        X_Cord = x,
                                        Y_Cord = y,
                                        Z_Cord = z,
                                        CargoID = cargo.ID,
                                        CargoWeight = 0
                                    };
                                    int lotpositionID = newPosition.Insert();
                                    cargo.UpdatePositionID(lotpositionID);

                                    lotPosList.Add(newPosition);
                                    breakBool = true;
                                    break;
                                }
                            }
                            if (breakBool)
                                break;
                        }
                        if (breakBool)
                            break;
                    }

                }
            }
        }

        private List<Cargo> ToCargoList(DataView dw)
        {
            List<Cargo> cargoList = new List<Cargo>();
            foreach (DataRow row in dw.Table.Rows)
            {
                Cargo cargo = new Cargo
                {
                    ID = int.Parse(row["CargoID"].ToString()),
                    Name = row["Title"].ToString(),
                    Code = row["Code"].ToString(),
                    Weight = int.Parse(row["Weight"].ToString()),
                    AcceptanceDate = DateTime.Parse(row["AcceptanceDate"].ToString()),
                    TracktorID = int.Parse(row["TracktorID"].ToString()),
                    UserID = int.Parse(row["UserID"].ToString()),
                    LotPositionID = int.Parse(row["LotPositionID"].ToString()),
                    State = row["State"]?.ToString() ?? "Delivered", // Default to "Delivered" if State is null
                    Gate = row["Gate"]?.ToString()
                };

                cargoList.Add(cargo);
            }
            return cargoList;
        }
        #endregion

        #region TracktorApiCalls
        private bool SendUnesignedDockCraneCoordinates(int tracktorID, Crane crane)
        {
            var url = $"https://{Request.Host.Value}/api/Tracktor/SendUnesignedDockCraneCoordinates/{tracktorID}";
            var content = new StringContent(JsonConvert.SerializeObject(crane), Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.PutAsync(url, content).Result;
                return response.IsSuccessStatusCode;
            }
        }
        private bool SendCoordinatesWithLoad(int tracktorID, CargoCrane cargoCrane)
        {
            var url = $"https://{Request.Host.Value}/api/Tracktor/SendCoordinatesWithLoad/{tracktorID}";
            var content = new StringContent(JsonConvert.SerializeObject(cargoCrane), Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.PutAsync(url, content).Result;
                return response.IsSuccessStatusCode;
            }
        }
        
        private bool SendTracktorHome(int tracktorID)
        {
            var url = $"https://{Request.Host.Value}/api/Tracktor/GoHome/{tracktorID}";
            var content = new StringContent("", Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.PutAsync(url, content).Result;
                return response.IsSuccessStatusCode;
            }
        }
        #endregion

        #region CraneOperatorJobs
        
        
        #endregion
    }
}
