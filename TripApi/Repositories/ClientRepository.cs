using Microsoft.EntityFrameworkCore;
using TripApi.Models;

namespace TripApi.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly TripDbContext _context;

        public ClientRepository(TripDbContext context)
        {
            _context = context;
        }

        public async Task<Client?> GetClientByIdAsync(int id)
        {
            return await _context.Clients
                .Include(c => c.ClientTrips)
                .FirstOrDefaultAsync(c => c.IdClient == id);
        }

        public async Task<Client?> GetClientByPeselAsync(string pesel)
        {
            return await _context.Clients
                .FirstOrDefaultAsync(c => c.Pesel == pesel);
        }

        public async Task<bool> ClientHasTripsAsync(int clientId)
        {
            return await _context.ClientTrips
                .AnyAsync(ct => ct.IdClient == clientId);
        }

        public async Task<bool> ClientRegisteredForTripAsync(string pesel, int tripId)
        {
            return await _context.ClientTrips
                .Include(ct => ct.IdClientNavigation)
                .AnyAsync(ct => ct.IdClientNavigation.Pesel == pesel && ct.IdTrip == tripId);
        }

        public async Task DeleteClientAsync(Client client)
        {
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
        }

        public async Task<Client> AddClientAsync(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task AddClientToTripAsync(ClientTrip clientTrip)
        {
            _context.ClientTrips.Add(clientTrip);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}