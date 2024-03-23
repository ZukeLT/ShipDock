namespace ShipDock.Models
{
    public class User
    {
        public int ID { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public Role Role { get; set; }
        public string ToRoleString()
        {
            switch (this.Role)
            {
                case Role.None:
                    return "Niekas";
                case Role.Client:
                    return "Klientas";
                case Role.Dispetcher:
                    return "Dispetčeris";
                case Role.Worker:
                    return "Darbininkas";
                case Role.Admin:
                    return "Administratorius";
                default:
                    return "WTF?";
            }
        }
    }
}
    public enum Role{
        None = 0,
        Client = 1,
        Dispetcher = 2,
        Worker = 3,
        Admin = 4
    }

