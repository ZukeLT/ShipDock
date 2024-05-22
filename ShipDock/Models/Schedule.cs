using ShipDock.Helper;

namespace ShipDock.Models
{
	public class Schedule
	{
		public DateTime? LeaveDate { get; set; }
		public DateTime? ArriveDate { get; set; }
		public int ShipID { get; set; }

		public int TripID { get; set; }
		public int RouteID { get; set; }
		public int DockID { get; set; }
		public string ShipModel { get; set; }
		public string From { get; set; }
		public string To { get; set; }
		public string DepartureStatus { get; set; }

		public bool Insert()
		{
			return DataSource.UpdateDataSQL($"INSERT INTO [dbo].[Trip] ([LeaveDate],[ArriveDate],[RouteID],[DockID],[ShipID])"
				+ $"VALUES ('{LeaveDate}', '{ArriveDate}', '{RouteID}', '{DockID}', '{ShipID}')");
		}
	}
}
