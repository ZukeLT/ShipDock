namespace ShipDock.Models
{
    public class Tracktor
    {
        public int TracktorID { get; set; }
        public string? Model { get; set; }
        public string Status { get; set; }  // e.g., "Waiting", "InTransit", "Arrived"
    }
}
