using Microsoft.AspNetCore.Mvc;
using ShipDock.Helper;
using ShipDock.Models;
using System.Data;
using System.Diagnostics;

namespace ShipDock.Controllers.Client
{
    public class ClientController : Controller
    {
        public List<Cargo> CargoList()
        {
            List<Cargo> cargoList = new List<Cargo>();
            DataView dw = (DataView)DataSource.ExecuteSelectSQL("SELECT * FROM Cargo");
            if (dw != null)
            {
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
            }
            return cargoList;
        }

        public IActionResult ViewCargoState()
        {
            var cargos = CargoList();
            return View(cargos);
        }

        public IActionResult RegisterCargo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterCargo(Cargo cargo)
        {
            if (cargo.Insert()) {
                TempData["SuccessMessage"] = "Cargo registered successfully.";
                return RedirectToAction(nameof(ViewCargoState));
            }
            else {
                TempData["ErrorMessage"] = "Failed to register cargo.";
                return View(cargo);
            }
        }

        public IActionResult CancelRegistration()
        {
            var cargos = CargoList();
            return View(cargos);
        }

        [HttpPost]
        public IActionResult CancelRegistration(int id)
        {
            string sql = $"UPDATE Cargo SET State = 'Canceled' WHERE CargoID = {id} AND State = 'New'";
            bool isValid = DataSource.UpdateDataSQL(sql);

            if (isValid)
            {
                TempData["SuccessMessage"] = "Cargo registration canceled successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Cargo can no longer be canceled.";
                return RedirectToAction(nameof(CancelRegistration));
            }
            return RedirectToAction(nameof(ViewCargoState));
        }

        public IActionResult PreviewCargo(int id)
        {
            var cargo = CargoList().FirstOrDefault(c => c.ID == id);
            if (cargo == null)
            {
                TempData["ErrorMessage"] = "Cargo not found.";
                return RedirectToAction(nameof(ViewCargoState));
            }
            return View(cargo);
        }

        public IActionResult CollectCargo()
        {
            var deliveredCargos = CargoList().Where(c => c.State == "Delivered" || c.State == "Prepared" || c.State == "Collected").ToList();
            return View(deliveredCargos);
        }

        [HttpPost]
        public IActionResult PrepareCargo(int id)
        {
            string sql = $"UPDATE Cargo SET State = 'Prepared' WHERE CargoID = {id}";
            bool isValid = DataSource.UpdateDataSQL(sql);

            if (isValid)
            {
                TempData["SuccessMessage"] = "Cargo prepared successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Cargo not found.";
            }
            return RedirectToAction(nameof(CollectCargo));
        }

        [HttpPost]
        public IActionResult ShowGateInfo(int id)
        {
            // Generate random gate
            string gateInfo = GenerateRandomGate();

            // Update Cargo table and set gateInfo in the Gate column
            string sql = $"UPDATE Cargo SET Gate = '{gateInfo}' WHERE CargoID = {id}";
            bool isValid = DataSource.UpdateDataSQL(sql);

            if (isValid)
            {
                TempData["SuccessMessage"] = "Gate information displayed.";
                TempData["GateInfo_" + id] = gateInfo; // Store gate info specific to cargo ID
            }
            else
            {
                TempData["ErrorMessage"] = "Cargo not found.";
            }
            return RedirectToAction(nameof(CollectCargo));
        }

        [HttpPost]
        public IActionResult ConfirmCollectedCargo(int id)
        {
            string sql = $"UPDATE Cargo SET State = 'Collected' WHERE CargoID = {id}";
            bool isValid = DataSource.UpdateDataSQL(sql);

            if (isValid)
            {
                TempData["SuccessMessage"] = "Cargo state updated to 'Collected'.";
            }
            else
            {
                TempData["ErrorMessage"] = "Cargo not found.";
            }
            return RedirectToAction(nameof(CollectCargo));
        }

        // Method to generate random gate
        private string GenerateRandomGate()
        {
            // Define gates A1 to F3
            string[] gates = { "A", "B", "C", "D", "E", "F" };
            Random rand = new Random();

            // Generate random gate index (0 to 5 for letters A to F)
            int letterIndex = rand.Next(0, 6);
            // Generate random number (1 to 3)
            int number = rand.Next(1, 4);

            // Concatenate letter and number to form gate
            return $"Gate {gates[letterIndex]}{number}";
        }

        [HttpPost]
        public IActionResult DeleteCargo(int id)
        {
            // Check if cargo exists and its state is "Canceled"
            var cargo = CargoList().FirstOrDefault(c => c.ID == id && c.State == "Canceled");
            if (cargo != null)
            {
                // Delete cargo from database
                bool deleted = DataSource.UpdateDataSQL($"DELETE FROM Cargo WHERE CargoID = {id}");
                if (deleted)
                {
                    TempData["SuccessMessage"] = "Cargo deleted successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete cargo.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Cargo not found or cannot be deleted.";
            }

            // Redirect back to the cargo list view
            return RedirectToAction("ViewCargoState");
        }
    }
}
