using APBD_05.DTO;
using Microsoft.AspNetCore.Mvc;

namespace APBD_05.Services;

public interface ITripService
{
    Task <List<TripDTO>> GetTrips();
    Task <IActionResult> AddClientToTrip(int idTrip, ClientTripDTO clientTripDto);
}