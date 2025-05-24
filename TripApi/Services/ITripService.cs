using TripApi.DTOs;

namespace TripApi.Services
{
    public interface ITripService
    {
        Task<TripResponseDto> GetTripsAsync(int page, int pageSize);
        Task<string> AssignClientToTripAsync(int tripId, AssignClientToTripDto dto);
    }
}