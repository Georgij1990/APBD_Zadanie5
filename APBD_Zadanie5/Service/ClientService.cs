using APBD_05.Context;
using Microsoft.AspNetCore.Mvc;

namespace APBD_05.Services;

public class ClientService : IClientService
{
    private readonly Context.Context _context;

    public ClientService(Context.Context context)
    {
        _context = context;
    }

    public async Task<IActionResult> DeleteClient(int id)
    {
        var clientTrip = _context.ClientTrips.FirstOrDefault(x => x.IdClient == id);
        if (clientTrip != null)
        {
            return new BadRequestObjectResult("Client is already enrolled on the trip.");
        }

        var client = await _context.Clients.FindAsync(id);
        if (client == null)
        {
            return new NotFoundObjectResult("Client has not found!");
        }

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();

        return new OkObjectResult("Client has been deleted!");
    }
    
}

