using TripApi.Models;

namespace TripApi.Repositories
{
    public interface ITripRepository
    {
        Task<(List<Trip> trips, int totalCount)> GetTripsWithPaginationAsync(int page, int pageSize);
        Task<Trip?> GetTripByIdAsync(int id);
        Task<bool> TripExistsAsync(int id);
    }
}