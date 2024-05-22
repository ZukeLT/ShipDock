using Microsoft.AspNetCore.Mvc;
using ShipDock.Models;

namespace ShipDock.Service
{
    public interface ITracktorService
    {
        bool AddTracktor(Tracktor tracktor);
        bool UpdateTracktorStatus(int id, string status);
    }

    public class TracktorService : ITracktorService
    {
        private readonly ITracktorRepository _tracktorRepository;

        public TracktorService(ITracktorRepository tracktorRepository)
        {
            _tracktorRepository = tracktorRepository;
        }

        public bool AddTracktor(Tracktor tracktor)
        {
            return _tracktorRepository.AddTracktor(tracktor);
        }

        public bool UpdateTracktorStatus(int id, string status)
        {
            return _tracktorRepository.UpdateTracktorStatus(id, status);
        }
    }

    public interface ICargoService
    {
        bool AddCargo(Cargo cargo);
        bool UpdateCargoStatus(int id, string status);
    }

    public class CargoService : ICargoService
    {
        private readonly ICargoRepository _cargoRepository;

        public CargoService(ICargoRepository cargoRepository)
        {
            _cargoRepository = cargoRepository;
        }

        public bool AddCargo(Cargo cargo)
        {
            return _cargoRepository.AddCargo(cargo);
        }

        public bool UpdateCargoStatus(int id, string status)
        {
            return _cargoRepository.UpdateCargoStatus(id, status);
        }
    }
}
