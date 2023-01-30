using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLTH.BL.RoomBL;

namespace QLTH.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly IRoomBL _roomBL;

    public RoomsController(IRoomBL roomBl)
    {
        _roomBL = roomBl;
    }

    [HttpGet]
    public async Task<ActionResult> GetRooms()
    {
        var rooms = await _roomBL.GetAllRecords();
        return Ok(rooms);
    }
}