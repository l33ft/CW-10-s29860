namespace TripApi.Services
{
    public interface IClientService
    {
        Task<string> DeleteClientAsync(int clientId);
    }
}