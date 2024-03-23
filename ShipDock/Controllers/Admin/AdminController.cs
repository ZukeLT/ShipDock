using Microsoft.AspNetCore.Mvc;
using ShipDock.Models;
using System.Data;

namespace ShipDock.Controllers.Admin
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ShipSearch()
        {
            List<Ship> shipList = new List<Ship>();
            for (int i = 0; i < 10; i++)
            {
                Ship ship = new Ship();
                ship.ID = i;
                ship.Model = "Test" + i.ToString();
                ship.Length = i+10;
                ship.Width = i + 5;
                ship.Height = i + 1;
                shipList.Add(ship);
            }

            return View(shipList);
        }
        public IActionResult UserSearch()
        {
            List<User> userList = new List<User>();
            for (int i = 0; i < 10; i++)
            {
                User user = new User();
                user.ID = i;
                user.Username = "Testasvicius" + i.ToString();
                user.Role = Role.Client;
                user.Password = "Test" + i.ToString();
                userList.Add(user);
            }

            return View(userList);
        }
        public IActionResult LotSearch()
        {
            List<Lot> lotList = new List<Lot>();
            for (int i = 0; i < 10; i++)
            {
                Lot lot = new Lot();
                lot.ID = i;
                lot.Code = "testas" + i.ToString();
                lot.X_Dim = 10 + i;
                lot.Y_Dim = 20 + i;
                lot.Z_Dim = 2 + i;
                lotList.Add(lot);
            }

            return View(lotList);
        }
    }
}
