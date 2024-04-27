using ShipDock.Helper;
using System.Reflection;

namespace ShipDock.Models
{
    public class User
    {
        public int ID { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? UserRole { get; set; }
        public bool Insert()
        {
            if (ID == 0)
            {
                return DataSource.UpdateDataSQL($"INSERT INTO [dbo].[User] ([Username],[Password],[UserRole]) " +
                    $"VALUES ('{Username}','{Password}','{UserRole}')");
            }
            return false;
        }
    }
}

