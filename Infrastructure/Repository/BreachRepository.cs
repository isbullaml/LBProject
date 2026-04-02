using Core.Entities;
using Core.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository
{
    public class BreachRepository :IBreachRepository
    {
        private readonly SqlService _sqlService;
        public BreachRepository(SqlService sqlService)
        {
            _sqlService = sqlService;
        }

        public async Task<IEnumerable<Breach>> GetAllAsync()
        {
            return await _sqlService.QueryAsync<Breach>(
                "SELECT * FROM Breaches",
                MapReaderToBreach,
                null
            );

        }

        public async Task<Breach?> GetByIdAsync(string id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@Id", id);

            return await _sqlService.QuerySingleAsync<Breach>(
                "SELECT * FROM Breaches WHERE Id = @Id",
                MapReaderToBreach,
                parameters
            );
        }

        // Helper method to map SqlDataReader to Breach object
        private Breach MapReaderToBreach(SqlDataReader reader)
        {
            return new Breach
            {
                Id = reader["Id"].ToString()!,
                Title = reader["Title"].ToString()!,
                Domain = reader["Domain"].ToString()!,
                BreachDate = Convert.ToDateTime(reader["BreachDate"]),
                AddedDate = Convert.ToDateTime(reader["AddedDate"]),
                ModifiedDate = Convert.ToDateTime(reader["ModifiedDate"]),
                PwnCount = Convert.ToInt64(reader["PwnCount"]),
                Description = reader["Description"].ToString()!,
                LogoPath = reader["LogoPath"].ToString()!,
                Attribution = reader["Attribution"].ToString()!,
                DisclosureUrl = reader["DisclosureUrl"].ToString()!,
                DataClasses = reader["DataClasses"].ToString()?.Split(',').ToList() ?? new List<string>(),
                IsVerified = Convert.ToBoolean(reader["IsVerified"]),
                IsFabricated = Convert.ToBoolean(reader["IsFabricated"]),
                IsSensitive = Convert.ToBoolean(reader["IsSensitive"]),
                IsRetired = Convert.ToBoolean(reader["IsRetired"]),
                IsSpamList = Convert.ToBoolean(reader["IsSpamList"]),
                IsMalware = Convert.ToBoolean(reader["IsMalware"]),
                IsSubscriptionFree = Convert.ToBoolean(reader["IsSubscriptionFree"]),
                IsStealerLog = Convert.ToBoolean(reader["IsStealerLog"])
            };
        }
    }
}
