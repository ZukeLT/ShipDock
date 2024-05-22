using Microsoft.AspNetCore.Mvc;
using ShipDock.Helper;
using ShipDock.Models;
using System.Data;
using System.Diagnostics;

namespace ShipDock.Controllers.Client
{
    // public class ClientController : Controller
    // {
    //     //Action to show the list of cargos
    //     public IActionResult ViewCargoState()
    //     {
    //         //var cargos = _dbContext.Cargos.ToList(); // Fetch cargos from the database
    //         return View();
    //     }

    //     // Action to register a new cargo
    //     public IActionResult RegisterCargo()
    //     {
    //         return View();
    //     }

    //     // Action to cancel cargo registration
    //     public IActionResult CancelRegistration()
    //     {
    //         return View();
    //     }
    // }

    public class ClientController : Controller
    {
        private static List<Cargo> cargos = new List<Cargo>
        {
            new Cargo { ID = 1, Name = "Cargo 1", Weight = 100.5, Volume = 50.3, State = "Delivered" },
            new Cargo { ID = 2, Name = "Cargo 2", Weight = 200.0, Volume = 75.0, State = "Delivered" },
            new Cargo { ID = 3, Name = "Cargo 3", Weight = 150.2, Volume = 60.4, State = "Prepared" }
        };

        public IActionResult ViewCargoState()
        {
            return View(cargos);
        }

        public IActionResult RegisterCargo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterCargo(Cargo cargo)
        {
            if (ModelState.IsValid)
            {
                cargo.ID = cargos.Any() ? cargos.Max(c => c.ID) + 1 : 1;
                cargo.State = "Delivered"; // Set the state to "Delivered"
                cargos.Add(cargo);
                TempData["SuccessMessage"] = "Cargo registered successfully.";
                return RedirectToAction("ViewCargoState");
            }
            TempData["ErrorMessage"] = "Failed to register cargo.";
            return View(cargo);
        }

        public IActionResult CancelRegistration()
        {
            return View(cargos);
        }

        [HttpPost]
        public IActionResult CancelRegistration(int id)
        {
            var cargo = cargos.FirstOrDefault(c => c.ID == id);
            if (cargo != null)
            {
                cargos.Remove(cargo);
                TempData["SuccessMessage"] = "Cargo registration canceled successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Cargo not found.";
            }
            return RedirectToAction("ViewCargoState");
        }

        public IActionResult PreviewCargo(int id)
        {
            var cargo = cargos.FirstOrDefault(c => c.ID == id);
            if (cargo == null)
            {
                TempData["ErrorMessage"] = "Cargo not found.";
                return RedirectToAction("ViewCargoState");
            }
            return View(cargo);
        }

        public IActionResult CollectCargo()
        {
            // Get cargos with state "Delivered"
            var deliveredCargos = cargos.Where(c => c.State == "Delivered" || c.State == "Prepared" || c.State == "Collected").ToList();
            return View(deliveredCargos);
        }

        [HttpPost]
        public IActionResult PrepareCargo(int id)
        {
            var cargo = cargos.FirstOrDefault(c => c.ID == id);
            if (cargo != null)
            {
                cargo.State = "Prepared";
                TempData["SuccessMessage"] = "Cargo prepared successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Cargo not found.";
            }
            return RedirectToAction("CollectCargo");
        }

        [HttpPost]
        public IActionResult CollectCargo(int id)
        {
            var cargo = cargos.FirstOrDefault(c => c.ID == id);
            if (cargo != null)
            {
                // Assuming you have logic here to determine the gate information
                string gateInfo = "Gate A"; // Example gate information

                // Update cargo state to collected or any other desired state
                cargo.State = "Collected";
                TempData["SuccessMessage"] = "Cargo collected successfully.";
                TempData["GateInfo_" + cargo.ID] = gateInfo; // Store gate info specific to cargo ID
            }
            else
            {
                TempData["ErrorMessage"] = "Cargo not found.";
            }
            return RedirectToAction("CollectCargo");
        }


    }

}
