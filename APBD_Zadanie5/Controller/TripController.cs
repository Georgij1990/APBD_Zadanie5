using APBD_05.DTO;
using APBD_05.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_05.Controller;

[Route("/api/trips")]
[ApiController]
public class TripController : ControllerBase
{
    private readonly ITripService _iTripService;

    public TripController(ITripService tripService)
    {
        _iTripService = tripService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTrips()
    {
        var result = await _iTripService.GetTrips();
        return Ok(result);
    }

    [HttpPost("{id}/clients")]
    public async Task<IActionResult> AddClientToTrip(int id, [FromBody] ClientTripDTO clientTripDto)
    {
        var result = await _iTripService.AddClientToTrip(id,clientTripDto);
        return result;
    }
}