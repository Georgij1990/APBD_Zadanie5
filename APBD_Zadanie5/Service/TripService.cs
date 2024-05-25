using APBD_05.Context;
using APBD_05.DTO;
using APBD_05.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APBD_05.Services;

public class TripService : ITripService
{
    private readonly Context.Context _context;

    public TripService(Context.Context context)
    {
        _context = context;
    }

    public async Task<List <TripDTO>> GetTrips()
    {
        var trips = new List<TripDTO>();
        trips = await _context.Trips
            .Include(trip => trip.IdCountries)
            .Include(trip => trip.ClientTrips)
            .OrderByDescending(trip => trip.DateFrom)
            .Select(trip => new TripDTO
            {
                Name = trip.Name,
                Description = trip.Description,
                DateFrom = trip.DateFrom,
                DateTo = trip.DateTo,
                MaxPeople = trip.MaxPeople,
                countries = trip.IdCountries.Select(country => new CountryDTO
                {
                    Name = country.Name
                }),

                clients = trip.ClientTrips.Select(client => new ClientDTO
                {
                    FirstName = client.IdClientNavigation.FirstName,
                    LastName = client.IdClientNavigation.LastName,
                })
            }).ToListAsync();
        return trips;
    }

    public async Task <IActionResult> AddClientToTrip(int idTrip, ClientTripDTO clientTripDto)
    {
        var peselExist = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == clientTripDto.Pesel);
        if (peselExist == null)
        {
            var newClient = new Client
            {
                IdClient = _context.Clients.Select(c => c.IdClient).DefaultIfEmpty().Max() + 1,
                FirstName = clientTripDto.FirstName,
                LastName = clientTripDto.LastName,
                Email = clientTripDto.Email,
                Telephone = clientTripDto.Telephone,
                Pesel = clientTripDto.Pesel
            };
            await _context.Clients.AddAsync(newClient);
            await _context.SaveChangesAsync();
        }

        var clientTrip = await _context.ClientTrips
            .Include(c => c.IdClientNavigation)
            .FirstOrDefaultAsync(c => c.IdTrip == clientTripDto.IdTrip && c.IdClientNavigation.Pesel == clientTripDto.Pesel);

        if (clientTrip != null)
        {
            return new BadRequestObjectResult("Client is assigned to the trip.");
        }

        var trip = await _context.Trips.FirstOrDefaultAsync(c => c.IdTrip == clientTripDto.IdTrip);
        if (trip == null)
        {
            return new BadRequestObjectResult("There is no such trip.");
        }

        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == clientTripDto.Pesel);
        if (client == null)
        {
            return new BadRequestObjectResult("There is no such client.");
        }

        var addClientToTrip = new ClientTrip
        {
            IdClient = client.IdClient,
            IdTrip = clientTripDto.IdTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = clientTripDto.PaymentDate
        };

        await _context.ClientTrips.AddAsync(addClientToTrip);
        await _context.SaveChangesAsync();

        return new OkObjectResult("Client has been added!");

    }
}