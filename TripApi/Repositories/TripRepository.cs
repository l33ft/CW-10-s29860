using Microsoft.EntityFrameworkCore;
using TripApi.Models;

namespace TripApi.Repositories
{
    public class TripRepository : ITripRepository
    {
        private readonly TripDbContext _context;

        public TripRepository(TripDbContext context)
        {
            _context = context;
        }

        public async Task<(List<Trip> trips, int totalCount)> GetTripsWithPaginationAsync(int page, int pageSize)
        {
            var totalCount = await _context.Trips.CountAsync();
            
            var trips = await _context.Trips
                .OrderByDescending(t => t.DateFrom)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(t => t.IdCountries) // Many-to-many relationship
                .Include(t => t.ClientTrips)
                .ThenInclude(ct => ct.IdClientNavigation)
                .ToListAsync();

            return (trips, totalCount);
        }

        public async Task<Trip?> GetTripByIdAsync(int id)
        {
            return await _context.Trips
                .Include(t => t.ClientTrips)
                .FirstOrDefaultAsync(t => t.IdTrip == id);
        }

        public async Task<bool> TripExistsAsync(int id)
        {
            return await _context.Trips.AnyAsync(t => t.IdTrip == id);
        }
    }
}