using Microsoft.AspNetCore.Mvc;
using ShipDock.Helper;
using ShipDock.Models;
using ShipDock.Service;
using System.Data;
using System.Text.Json.Serialization;
using System.Text.Json;
using ShipDock.Models.agreg;
using ShipDock.Helpers;

[ApiController]
[Route("api/[controller]")]
public class TracktorController : ControllerBase
{
    #region justTestin
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

    [HttpPut("UpdateTracktorStatus/{id}")]
    public IActionResult UpdateTracktorStatus(int id, [FromBody] Tracktor tracktor)
    {
        var query = $"UPDATE Tracktor SET [Status] = '{tracktor.Status}' WHERE TracktorID = {id}";
        var isSuccess = DataSource.UpdateDataSQL(query);
        return isSuccess ? Ok() : StatusCode(500);
    }

    private List<Dictionary<string, object>> ConvertDataViewToList(DataView dataView)
    {
        var list = new List<Dictionary<string, object>>();

        foreach (DataRowView rowView in dataView)
        {
            var row = rowView.Row;
            var dict = row.Table.Columns.Cast<DataColumn>()
                            .ToDictionary(col => col.ColumnName, col => row[col]);
            list.Add(dict);
        }

        return list;
    }
    [HttpGet]
    public IActionResult GetAllTracktors()
    {
        var data = DataSource.ExecuteSelectSQL("SELECT * FROM Tracktor");
        return Ok(ConvertDataViewToList(data));
    }
    #endregion


    [HttpPut("SendUnesignedDockCraneCoordinates/{id}")]
    public IActionResult SendUnesignedDockCraneCoordinates(int id, [FromBody] Crane crane)
    {
        var query = 
            $"UPDATE Tracktor SET [CraneID] = {crane.CraneID}, " +
            $"[CargoID] = NULL, " +
            $"[Status] = '{Enums.TracktorStates.Loading}' " + 
            $"WHERE TracktorID = {id}";
        var isSuccess = DataSource.UpdateDataSQL(query);

        Task.Run(async () =>
        {
            await Task.Delay(10000); //10 sec duodam kol atvara

            var updateQuery =
                $"UPDATE Tracktor " +
                $"SET [Status] = '{Enums.TracktorStates.Waiting}' " +
                $"WHERE TracktorID = {id}";
            DataSource.UpdateDataSQL(updateQuery);
        });

        return isSuccess ? Ok() : StatusCode(500);
    }
    [HttpPut("SendCoordinatesWithLoad/{id}")]
    public IActionResult SendCoordinatesWithLoad(int id, [FromBody] CargoCrane cargoCrane)
    {
        var query =
            $"UPDATE Tracktor " +
            $"SET [CraneID] = {cargoCrane.crane.CraneID}, " +
            $"[CargoID] = {cargoCrane.cargo.ID}, " +
            $"[Status] = '{Enums.TracktorStates.Unloading}' " +
            $"WHERE TracktorID = {id}";
        var isSuccess = DataSource.UpdateDataSQL(query);

        Task.Run(async () =>
        {
            await Task.Delay(10000); //10 sec duodam kol atvara

            var updateQuery =
                $"UPDATE Tracktor " +
                $"SET [Status] = '{Enums.TracktorStates.Waiting}' " +
                $"WHERE TracktorID = {id}";
            DataSource.UpdateDataSQL(updateQuery);
        });

        return isSuccess ? Ok() : StatusCode(500);
    }
   
    [HttpPut("GoHome/{id}")]
    public IActionResult GoHome(int id)
    {
        var query =
        $"UPDATE Tracktor SET [CraneID] = NULL, " +
        $"[CargoID] = NULL, " +
        $"[Status] = '{Enums.TracktorStates.Returning}' , " +
        $"WHERE TracktorID = {id}";
        var isSuccess = DataSource.UpdateDataSQL(query);

        Task.Run(async () =>
        {
            await Task.Delay(10000); //10 sec duodam kol grista ir naujinam i nauja statusa

            var updateQuery =
                $"UPDATE Tracktor " +
                $"SET [Status] = '{Enums.TracktorStates.Waiting}' " +
                $"WHERE TracktorID = {id}";
            DataSource.UpdateDataSQL(updateQuery);
        });

        return isSuccess ? Ok() : StatusCode(500);
    }
}
