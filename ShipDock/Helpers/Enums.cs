namespace ShipDock.Helpers
{
    public class Enums
    {
        public enum CargoStates
        {
            New,
            Canceled,
            Pending,
            Unloading,
            Proccesing,
            Delivered,
            Prepared,
            Collected
        }
        public enum TracktorStates
        {
            Waiting,
            Loading,
            Unloading,
            Returning
        }
        public enum UserRoles
        {
            Administratorius,
            Dispeciaris,
            Darbuotojas,
            Klientas,
            Iškrovimo_Darbuotojas,
            Pakrovimo_Darbuotojas
        }
        public static string ToTracktorState(CargoStates state)
        {
            switch (state)
            {
                case CargoStates.New:
                    return "New";
                case CargoStates.Canceled:
                    return "Canceled";
                case CargoStates.Pending:
                    return "Pending";
                case CargoStates.Unloading:
                    return "Unloading";
                case CargoStates.Proccesing:
                    return "Proccesing";
                case CargoStates.Delivered:
                    return "Delivered";
                case CargoStates.Prepared:
                    return "Prepared";
                case CargoStates.Collected:
                    return "Collected";
                default:
                    return "";
            }
        }
        public static string ToTracktorState(TracktorStates state)
        {
            switch (state) 
            {
                case TracktorStates.Waiting:
                    return "Waiting";
                case TracktorStates.Loading:
                    return "Loading";
                case TracktorStates.Unloading:
                    return "Unloading";
                case TracktorStates.Returning:
                    return "Returning";
                default:
                    return "";
            }
        }
    }
   

}
