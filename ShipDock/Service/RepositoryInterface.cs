using Microsoft.AspNetCore.Mvc;
using ShipDock.Helper;
using ShipDock.Models;

namespace ShipDock.Service
{
    public interface ITracktorRepository
    {
        bool AddTracktor(Tracktor tracktor);
        bool UpdateTracktorStatus(int id, string status);
    }

    public class TracktorRepository : ITracktorRepository
    {
        public bool AddTracktor(Tracktor tracktor)
        {
            string sql = $"INSERT INTO [dbo].[Tracktor] ([Model], [Status]) VALUES ('{tracktor.Model}', '{tracktor.Status}')";
            return DataSource.UpdateDataSQL(sql);
        }

        public bool UpdateTracktorStatus(int id, string status)
        {
            string sql = $"UPDATE [dbo].[Tracktor] SET [Status] = '{status}' WHERE [TracktorID] = {id}";
            return DataSource.UpdateDataSQL(sql);
        }
    }

    public interface ICargoRepository
    {
        bool AddCargo(Cargo cargo);
        bool UpdateCargoStatus(int id, string status);
    }

    public class CargoRepository : ICargoRepository
    {
        public bool AddCargo(Cargo cargo)
        {
            string sql = $"INSERT INTO [dbo].[Cargo] ([Status], [TracktorID]) VALUES ('{cargo.State}', {cargo.TracktorID})";
            return DataSource.UpdateDataSQL(sql);
        }

        public bool UpdateCargoStatus(int id, string status)
        {
            string sql = $"UPDATE [dbo].[Cargo] SET [Status] = '{status}' WHERE [CargoID] = {id}";
            return DataSource.UpdateDataSQL(sql);
        }
    }

}
