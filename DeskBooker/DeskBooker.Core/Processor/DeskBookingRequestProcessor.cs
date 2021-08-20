using System;
using DeskBooker.Core.DataInterface;
using DeskBooker.Core.Domain;

namespace DeskBooker.Core.Processor
{
    public class DeskBookingRequestProcessor
    {
        private readonly IDeskBookingRepository _deskBookingRepository;

        public DeskBookingRequestProcessor(IDeskBookingRepository deskBookingRepository)
        {
            _deskBookingRepository = deskBookingRepository;
        }

        public DeskBookingResult BookDesk(DeskBookingRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            _deskBookingRepository.Save(new DeskBooking
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Date = request.Date
            });

            return new DeskBookingResult
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Date = request.Date
            };
        }
    }
}