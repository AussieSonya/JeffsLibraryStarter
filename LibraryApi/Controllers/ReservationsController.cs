using AutoMapper;
using AutoMapper.QueryableExtensions;
using LibraryApi.Domain;
using LibraryApi.Filters;
using LibraryApi.Models.Reservations;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class ReservationsController : ControllerBase
    {
        private readonly LibraryDataContext _context;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _config;
        private readonly ILogReservations _reservationLogger;

        public ReservationsController(LibraryDataContext context, IMapper mapper, MapperConfiguration config, ILogReservations reservationLogger)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
            _reservationLogger = reservationLogger;
        }

        [HttpPost("reservations")]
        [ValidateModel]
        public async Task<ActionResult> AddReservation([FromBody] PostReservationRequest request)
        {
            var reservation = _mapper.Map<Reservation>(request);
            reservation.Status = ReservationStatus.Pending;
            _context.Reservations.Add(reservation);
            
            await _context.SaveChangesAsync();
            var response = _mapper.Map<ReservationDetailsResponse>(reservation);
            await _reservationLogger.WriteAsync(reservation);
           
            return CreatedAtRoute("reservations#getbyid", new { id = response.Id }, response);
            
        }


        [HttpGet("reservations/{id}", Name ="reservations#getbyid")]
        public async Task<ActionResult> GetReservationById(int id)
        {
            var reservation = await _context.Reservations
                .ProjectTo<ReservationDetailsResponse>(_config)
                .SingleOrDefaultAsync(r => r.Id == id);

            return this.Maybe(reservation);
        }
    }
}

