using System.Data;
using System.Data.SqlClient;

namespace ShipDock.Helper
{
    public static class DataSource
    {
        /// <summary>
        /// Pagal procedūra gražina duomenis vartotojui. Naudojama duomenų parinkimui (SELECT). Kaip DB užvadinata lentelės gražinamas stilpelis, taip ir čia bus jis vadinamas.
        /// </summary>
        /// <param name="procedureName">Procedūros pavadinimas</param>
        /// <param name="parameters">parametrai</param>
        /// <returns>Lenta užpildyta gražinamais duomenimis</returns>
        public static DataView SelectData(string procedureName, string[] parameters)
        {
            string connectionString = Constants.ConnectionString;

            try 
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        for (int i = 0; i < parameters.Length; i += 2)
                        {
                            string paramName = parameters[i];
                            string paramValue = parameters[i + 1];

                            command.Parameters.AddWithValue(paramName, paramValue);
                        }

                        DataTable dataTable = new DataTable();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);

                        connection.Open();
                        adapter.Fill(dataTable);

                        return dataTable.DefaultView;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing command: {ex.Message}");
                return null;
            }

            return null;
        }

        /// <summary>
        /// Galima naudoti kviesti procedura kuri modifikuoja duomenis (UPDATE,DELETE ir t.t.). Jei procedūra pavyko įvykdyti gražina true. Jei nepavyko, tai False;
        /// </summary>
        /// <param name="procedureName">P{rocedūros pavadinimas</param>
        /// <param name="parameters">parametrai</param>
        /// <returns>True arba False, Priklausomai ar pasisekė</returns>
        public static bool UpdateData(string procedureName, string[] parameters)
        {
            string connectionString = Constants.ConnectionString;
          
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        for (int i = 0; i < parameters.Length; i += 2)
                        {
                            string paramName = parameters[i];
                            string paramValue = parameters[i + 1];

                            command.Parameters.AddWithValue(paramName, paramValue);
                        }

                        connection.Open();
                        command.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing stored procedure: {ex.Message}");
                return false;
            } 
        }

        /// <summary>
        /// Skirta jei yra noras dirbti be procedūrų. paduodame SQL kodo eilutę ir iš eilutės gauname duomenis. Naudojame tik duomenų gavimui o ne modifikacijai
        /// </summary>
        /// <param name="CommandText">SQL kodo tekstas</param>
        /// <returns>Užpildyta duomenų lentelė</returns>
        public static DataView ExecuteSelectSQL(string CommandText)
        {
            string connectionString = Constants.ConnectionString;
            
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(CommandText, connection))
                    {
                        DataTable dataTable = new DataTable();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);

                        connection.Open();
                        adapter.Fill(dataTable);

                        return dataTable.DefaultView;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing command: {ex.Message}");
                return null;
            }
        }
        


        /// <summary>
        /// Atnaujina duomenis pagal SQL pateikta kodą
        /// </summary>
        /// <param name="CommandText">SQL querry</param>
        /// <returns>True or False</returns>
        public static bool UpdateDataSQL(string CommandText)
        {
            string connectionString = Constants.ConnectionString;   
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(CommandText, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing command: {ex.Message}");
                return false;
            }
        }

        public static int InsertSQL(string CommandText)
        {
            string connectionString = Constants.ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(CommandText, connection))
                    {
                        connection.Open();

                        command.CommandText += "; SELECT SCOPE_IDENTITY();";

                        int identity = Convert.ToInt32(command.ExecuteScalar());
                        return identity;
                    }
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {

            }
        }
    }
}
