namespace ShipDock.Models
{
    public class Route
    {
        public int RouteID { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int Distance { get; set; }

        public Route(int routeID, string from, string to, int distance) {
            this.RouteID = routeID;
            this.From = from;
            this.To = to;
            this.Distance = distance;
        }
    }
}
