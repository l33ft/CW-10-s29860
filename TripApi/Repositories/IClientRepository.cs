using TripApi.Models;

namespace TripApi.Repositories
{
    public interface IClientRepository
    {
        Task<Client?> GetClientByIdAsync(int id);
        Task<Client?> GetClientByPeselAsync(string pesel);
        Task<bool> ClientHasTripsAsync(int clientId);
        Task<bool> ClientRegisteredForTripAsync(string pesel, int tripId);
        Task DeleteClientAsync(Client client);
        Task<Client> AddClientAsync(Client client);
        Task AddClientToTripAsync(ClientTrip clientTrip);
        Task SaveChangesAsync();
    }
}