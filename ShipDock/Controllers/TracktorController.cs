using Microsoft.AspNetCore.Mvc;
using ShipDock.Helper;
using ShipDock.Models;
using ShipDock.Service;

[ApiController]
[Route("api/[controller]")]
public class TracktorController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllTracktors()
    {
        var data = DataSource.ExecuteSelectSQL("SELECT * FROM Tracktor");
        return Ok(data);
    }

    [HttpGet("{id}")]
    public IActionResult GetTracktorById(int id)
    {
        var data = DataSource.ExecuteSelectSQL($"SELECT * FROM Tracktor WHERE TracktorID = {id}");
        return Ok(data);
    }

    [HttpPost]
    public IActionResult CreateTracktor([FromBody] Tracktor tracktor)
    {
        var query = $"INSERT INTO Tracktor (Model, Status) VALUES ({tracktor.Model}, {tracktor.Status})";
        var isSuccess = DataSource.UpdateDataSQL(query);
        return isSuccess ? Ok() : StatusCode(500);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateTracktorStatus(int id, [FromBody] Tracktor tracktor)
    {
        var query = $"UPDATE Tracktor SET Status = {tracktor.Status} WHERE TracktorID = {id}";
        var isSuccess = DataSource.UpdateDataSQL(query);
        return isSuccess ? Ok() : StatusCode(500);
    }
}

[ApiController]
[Route("api/[controller]")]
public class LotPositionController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllLotPositions()
    {
        var data = DataSource.ExecuteSelectSQL("SELECT * FROM LotPosition");
        return Ok(data);
    }
}

[ApiController]
[Route("api/[controller]")]
public class CraneController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllCranes()
    {
        var data = DataSource.ExecuteSelectSQL("SELECT * FROM Crane");
        return Ok(data);
    }
}

[ApiController]
[Route("api/[controller]")]
public class CargoLoadController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllCargoLoads()
    {
        var data = DataSource.ExecuteSelectSQL("SELECT * FROM CargoLoad");
        return Ok(data);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateCargoLoadStatus(int id, [FromBody] Cargo cargo)
    {
        var query = $"UPDATE CargoLoad SET Status = {cargo.State} WHERE CargoLoadID = {id}";
        var isSuccess = DataSource.UpdateDataSQL(query);
        return isSuccess ? Ok() : StatusCode(500);
    }
}

[Route("api/[controller]")]
[ApiController]
public class CargoController : ControllerBase
{
    private readonly ICargoService _cargoService;

    public CargoController(ICargoService cargoService)
    {
        _cargoService = cargoService;
    }

    [HttpPost]
    public IActionResult CreateCargo([FromBody] Cargo cargo)
    {
        var result = _cargoService.AddCargo(cargo);
        if (result) return Ok();
        return BadRequest();
    }

    [HttpPut("{id}")]
    public IActionResult UpdateCargoStatus(int id, [FromBody] string status)
    {
        var result = _cargoService.UpdateCargoStatus(id, status);
        if (result) return Ok();
        return NotFound();
    }
}
