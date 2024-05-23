using ShipDock.Helper;
using System;
using System.Data;
using System.Linq;

namespace ShipDock.Models
{
    public class Cargo
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public double Weight { get; set; }
        public DateTime AcceptanceDate { get; set; }
        public int TracktorID { get; set; }
        public int UserID { get; set; }
        public int LotPositionID { get; set; }
        public string? State { get; set; }
        public string? Gate { get; set; }

        private static Random random = new Random();

        public bool Insert()
        {
            if (ID == 0)
            {
                Code = GenerateUniqueCode();
                return DataSource.UpdateDataSQL($"INSERT INTO [dbo].[Cargo] ([Title],[Code],[Weight],[AcceptanceDate],[TracktorID],[UserID],[LotPositionID]) " +
                    $"VALUES ('{Name}','{Code}',{Weight},'{AcceptanceDate}',{TracktorID},{UserID},{LotPositionID})");
            }
            return false;
        }

        private string GenerateUniqueCode()
        {
            string newCode;
            do
            {
                newCode = GenerateRandomCode();
            } while (IsCodeExistsInDatabase(newCode));
            return newCode;
        }

        private string GenerateRandomCode()
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char letter = letters[random.Next(letters.Length)];
            int number = random.Next(1, 100000000); // Generate a number between 1 and 99999999
            return $"{letter}{number:D8}"; // D8 formats the number to 8 digits
        }

        private bool IsCodeExistsInDatabase(string code)
        {
            string sql = $"SELECT COUNT(*) AS CNT FROM [dbo].[Cargo] WHERE [Code] = '{code}'";
            DataView number = DataSource.ExecuteSelectSQL(sql);
            return Convert.ToInt32(number.Table.Rows[0]["CNT"]) > 0;
        }
    }
}
