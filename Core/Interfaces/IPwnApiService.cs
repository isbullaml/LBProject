using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    public interface IPwnApiService
    {
        Task<IEnumerable<PwnBreach>> GetBreachesAsync();
        Task<PwnBreach?> GetBreachByNameAsync(string name);
    }
}
