using Microsoft.AspNetCore.Mvc;
using ShipDock.Helper;
using ShipDock.Models;
using System.Data;

namespace ShipDock.Controllers.Control
{
	public class ScheduleController : Controller
	{
		public IActionResult Schedule()
		{
			List<Schedule> scheduleList = new List<Schedule>();
			DataView dw = (DataView)DataSource.ExecuteSelectSQL("SELECT t.TripID, t.ArriveDate, t.LeaveDate, t.DepartureStatus, s.ShipID, s.Model, r.[From], r.[To] FROM Trip t JOIN Ship s ON t.ShipID = s.ShipID JOIN [Route] r ON t.RouteID = r.RouteID;");

			if(dw != null)
			{
				foreach(DataRow row in dw.Table.Rows)
				{
					Schedule schedule = new Schedule();
					schedule.TripID = int.Parse(row["TripID"].ToString());
					schedule.LeaveDate = DateTime.Parse(row["LeaveDate"].ToString());
					schedule.ArriveDate = DateTime.Parse(row["ArriveDate"].ToString());
					schedule.ShipID = int.Parse(row["ShipID"].ToString());
					schedule.ShipModel = row["Model"].ToString();
					schedule.From = row["From"].ToString();
					schedule.To = row["To"].ToString();
					schedule.DepartureStatus = row["DepartureStatus"].ToString();
					scheduleList.Add(schedule);
				}
			}
            return View(scheduleList);
        }

		public IActionResult SaveSchedule(Schedule schedule)
		{
			if (schedule.Insert())
				TempData["SuccessMessage"] = "Schedule Successfully inserted";
			else
				TempData["ErrorMessage"] = "Schedule Insertion Fail";

			return RedirectToAction("Schedule");
		}

		public IActionResult UpdateDepartureStatus(string id)
		{
			bool isValid = DataSource.UpdateDataSQL($"UPDATE Trip SET DepartureStatus = 'Departed' WHERE TripID = '{id}';");

			if (isValid)
				TempData["SuccessMessage"] = "Departure Successfully Confirmed";
			else
				TempData["ErrorMessage"] = "Departure Confirmation Failed";
			return RedirectToAction("Schedule");

		}
	}
}
