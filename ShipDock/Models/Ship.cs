using ShipDock.Helper;
using System.Data;

namespace ShipDock.Models
{
    public class Ship
    {
        public int ID { get; set; }
        public string? Model { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Insert()
        {
            if(ID == 0)
            {
                return DataSource.UpdateDataSQL($"INSERT INTO [dbo].[Ship] ([Model],[Length],[Width],[Height]) VALUES ('{Model}',{Length},{Width},{Height})");
            }
            return false;
        }
    }
}
