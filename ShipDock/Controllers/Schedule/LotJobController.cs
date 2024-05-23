using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShipDock.Helper;
using ShipDock.Helpers;
using ShipDock.Models;
using ShipDock.Models.agreg;
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
                $"WHERE [User].UserRole = '{"Pakrovimo Darbuotojas"}'"); //Reikiaa parinkti tokius kurie dar nera tarp tracktor objecto

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

        public void ConfirmCargoLoading(Cargo cargo)
        {
        }

        public void ConfirmCargoUnloading(Cargo cargo)
        {
        }

        private void CargoLoadAssignLots()
        {
            DataView CLP = DataSource.ExecuteSelectSQL("SELECT * FROM Cargo WHERE Status = 'Unloading'");
            if(CLP != null && CLP.Table.Rows.Count > 0)
            {
                DataView Lots = DataSource.ExecuteSelectSQL("SELECT * FROM Lot");
                DataView LotsPositions = DataSource.ExecuteSelectSQL("SELECT * FROM Lot");
                foreach(DataRow row in CLP)
                {
                    //Iterpiam row i lot positions kur laisva;
                }
            }
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
        #endregion

        #region CraneOperatorJobs
        public IActionResult LoadedCargo(string userJSON,string tracktorJSON, string cargoJSON)
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

                SendCoordinatesWithLoad(tracktor.TracktorID,cargoCrane);
            }

            

            Console.WriteLine("LoadedCargo action");
            return View("../Schedule/WorkerJobs");
        }
        public IActionResult UnloadedCargo(string userJSON, string tracktorJSON, string cargoJSON)
        {
            User user = JsonConvert.DeserializeObject<User>(userJSON);
            Tracktor tracktor = JsonConvert.DeserializeObject<Tracktor>(tracktorJSON);
            Cargo cargo = JsonConvert.DeserializeObject<Cargo>(cargoJSON);

            Console.WriteLine("UnloadedCargo action");


            var isSuccess1 = DataSource.UpdateDataSQL(
              $"UPDATE Tracktor " +
              $"SET [CraneID] = NULL, " +
              $"[CargoID] = NULL " +
              $"WHERE TracktorID = {tracktor.TracktorID}");

            var isSuccess2 = DataSource.UpdateDataSQL(
              $"UPDATE Cargo " +
              $"SET [State] = '{Enums.CargoStates.Prepared}' " +
              $"WHERE CargoID = {cargo.ID}");


            CheckTracktorStatuses();
            return View("../Schedule/WorkerJobs");
        }
        #endregion
    }
}
