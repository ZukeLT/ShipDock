using Microsoft.AspNetCore.Mvc;
using ShipDock.Helper;
using ShipDock.Models;
using System.Data;
using System.Diagnostics;

namespace ShipDock.Controllers.Admin
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        #region Ship Section
        public IActionResult ShipList()
        {
            List<Ship> shipList = new List<Ship>();
            DataView dw = (DataView)DataSource.ExecuteSelectSQL("SELECT * FROM Ship");
            if(dw!= null)
            {
                foreach(DataRow row in dw.Table.Rows)
                {
                    Ship ship = new Ship();
                    ship.ID = int.Parse(row["ShipID"].ToString());
                    ship.Model = row["Model"].ToString();
                    ship.Length = int.Parse(row["Length"].ToString());
                    ship.Width = int.Parse(row["Width"].ToString());
                    ship.Height = int.Parse(row["Height"].ToString());
                    shipList.Add(ship);
                }
            }

            return View(shipList);
        }
        public IActionResult DeleteShip(string id)
        {
            Debug.WriteLine("Testyas");
            bool isValid = DataSource.UpdateDataSQL($"DELETE FROM Ship WHERE ShipID = {id}");
            if (isValid)
                TempData["SuccessMessage"] = "Data Successfully Deleted";
            else
                TempData["ErrorMessage"] = "Data Deletion Fail";
            
            return RedirectToAction("ShipList");
        }
        public IActionResult SaveShip(Ship ship)
        {
            if (ship.Insert())
                TempData["SuccessMessage"] = "Data Successfully inserted";
            else
                TempData["ErrorMessage"] = "Data Insertion Fail";

            return RedirectToAction("ShipList");
        }
        #endregion

        #region User Section
        public IActionResult UserList()
        {
            List<User> userList = new List<User>();
            DataView dw = (DataView)DataSource.ExecuteSelectSQL("SELECT * FROM [User]");
            if (dw != null)
            {
                foreach (DataRow row in dw.Table.Rows)
                {
                    User user = new User();
                    user.ID = int.Parse(row["UserID"].ToString());
                    user.Username = row["Username"].ToString();
                    user.UserRole = row["UserRole"].ToString();
                    user.Password = row["Password"].ToString();
                    userList.Add(user);
                }
            }

            return View(userList);
        }

        public IActionResult DeleteUser(string id)
        {
            bool isValid = DataSource.UpdateDataSQL($"DELETE FROM [User] WHERE UserID = {id}");
            if (isValid)
                TempData["SuccessMessage"] = "Data Successfully Deleted";
            else
                TempData["ErrorMessage"] = "Data Deletion Fail";

            return RedirectToAction("UserList");
        }
        public IActionResult SaveUser(User user)
        {
            if (user.Insert())
                TempData["SuccessMessage"] = "Data Successfully inserted";
            else
                TempData["ErrorMessage"] = "Data Insertion Fail";

            return RedirectToAction("UserList");
        }
        #endregion

        #region Lot Section
        public IActionResult LotList()
        {
            List<Lot> lotList = new List<Lot>();
            DataView dw = (DataView)DataSource.ExecuteSelectSQL("SELECT * FROM Lot");
            for (int i = 0; i < 10; i++)
            {

            }
            if (dw != null)
            {
                foreach (DataRow row in dw.Table.Rows)
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

        public IActionResult DeleteLot(string id)
        {
            bool isValid = DataSource.UpdateDataSQL($"DELETE FROM Lot WHERE LotID = {id}");
            if (isValid)
                TempData["SuccessMessage"] = "Data Successfully Deleted";
            else
                TempData["ErrorMessage"] = "Data Deletion Fail";

            return RedirectToAction("LotList");
        }
        public IActionResult SaveLot(Lot lot)
        {
            if(lot.Insert())
                TempData["SuccessMessage"] = "Data Successfully inserted";
            else
                TempData["ErrorMessage"] = "Data Insertion Fail";

            return RedirectToAction("LotList");
        }
        #endregion
    }
}
