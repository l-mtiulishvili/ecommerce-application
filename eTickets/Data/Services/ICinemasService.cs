using eTickets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Data.Services
{
    public interface ICinemasService
    {
        Task<IEnumerable<Cinema>> GetallAsync();
        Task<Cinema> GetByIdAsync(int id);
        Task addAsync(Cinema cinema);
        Task<Cinema> UpdateAsync(int id, Cinema newCinema);
        Task DeleteAsync(int id);
    }
}
