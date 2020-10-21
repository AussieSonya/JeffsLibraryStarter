using LibraryApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Services
{
    public interface ILogReservations
    {
        Task WriteAsync(Reservation reservation);
    }
}
