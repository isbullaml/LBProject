using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    public interface IBreachRepository
    {
        Task<IEnumerable<Breach>> GetAllAsync();
        Task<Breach?> GetByIdAsync(string Id);

    }
}
