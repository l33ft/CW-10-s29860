using TripApi.DTOs;
using TripApi.Models;
using TripApi.Repositories;

namespace TripApi.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _tripRepository;
        private readonly IClientRepository _clientRepository;

        public TripService(ITripRepository tripRepository, IClientRepository clientRepository)
        {
            _tripRepository = tripRepository;
            _clientRepository = clientRepository;
        }

        public async Task<TripResponseDto> GetTripsAsync(int page, int pageSize)
        {
            var (trips, totalCount) = await _tripRepository.GetTripsWithPaginationAsync(page, pageSize);
            var allPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var tripDtos = trips.Select(t => new TripDto
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.IdCountries.Select(c => new CountryDto
                {
                    Name = c.Name
                }).ToList(),
                Clients = t.ClientTrips.Select(ct => new ClientDto
                {
                    FirstName = ct.IdClientNavigation.FirstName,
                    LastName = ct.IdClientNavigation.LastName
                }).ToList()
            }).ToList();

            return new TripResponseDto
            {
                PageNum = page,
                PageSize = pageSize,
                AllPages = allPages,
                Trips = tripDtos
            };
        }

        public async Task<string> AssignClientToTripAsync(int tripId, AssignClientToTripDto dto)
        {
            var existingClient = await _clientRepository.GetClientByPeselAsync(dto.Pesel);
            if (existingClient != null)
            {
                return "Klient o podanym numerze PESEL już istnieje";
            }

            var isAlreadyRegistered = await _clientRepository.ClientRegisteredForTripAsync(dto.Pesel, tripId);
            if (isAlreadyRegistered)
            {
                return "Klient jest już zapisany na tę wycieczkę";
            }

            var trip = await _tripRepository.GetTripByIdAsync(tripId);
            if (trip == null)
            {
                return "Wycieczka nie istnieje";
            }

            if (trip.DateFrom <= DateTime.Now)
            {
                return "Nie można zapisać się na wycieczkę, która już się odbyła";
            }

            var newClient = new Client
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Telephone = dto.Telephone,
                Pesel = dto.Pesel
            };

            var addedClient = await _clientRepository.AddClientAsync(newClient);

            var clientTrip = new ClientTrip
            {
                IdClient = addedClient.IdClient,
                IdTrip = tripId,
                RegisteredAt = DateTime.Now,
                PaymentDate = dto.PaymentDate
            };

            await _clientRepository.AddClientToTripAsync(clientTrip);

            return "Klient został pomyślnie przypisany do wycieczki";
        }
    }
}