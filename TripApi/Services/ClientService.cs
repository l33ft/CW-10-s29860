using TripApi.Repositories;

namespace TripApi.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<string> DeleteClientAsync(int clientId)
        {
            var client = await _clientRepository.GetClientByIdAsync(clientId);
            if (client == null)
            {
                return "Klient nie istnieje";
            }

            var hasTrips = await _clientRepository.ClientHasTripsAsync(clientId);
            if (hasTrips)
            {
                return "Nie można usunąć klienta, który ma przypisane wycieczki";
            }

            await _clientRepository.DeleteClientAsync(client);
            return "Klient został pomyślnie usunięty";
        }
    }
}