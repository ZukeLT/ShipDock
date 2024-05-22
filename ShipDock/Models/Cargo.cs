using ShipDock.Helper;
using System.Reflection;

namespace ShipDock.Models
{
    public class Cargo
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public double Weight { get; set; }
        public double Volume { get; set; }
        public string? State { get; set; }
        public int TracktorID { get; set; }
    }

}