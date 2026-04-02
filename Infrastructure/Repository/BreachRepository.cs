using Core.Entities;
using Core.Interfaces;
using Core.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repository
{
    public class BreachRepository : IBreachRepository
    {
        private readonly SqlService _sqlService;
        private readonly IPwnApiService _pwnApiService;
        public BreachRepository(SqlService sqlService, IPwnApiService pwnApiService)
        {
            _sqlService = sqlService;
            _pwnApiService = pwnApiService;
        }

        public async Task<IEnumerable<Breach>> GetAllAsync()
        {
            var localBreaches = (await _sqlService.QueryAsync<Breach>(
                "SELECT * FROM Breaches", MapReaderToBreach, null)).ToList();

            var remoteBreaches = await _pwnApiService.GetBreachesAsync();

            foreach (var remote in remoteBreaches)
            {
                var local = localBreaches.FirstOrDefault(b => b.Id == remote.Name);

                if (local == null)
                {
                    await AddAsync(remote);
                    localBreaches.Add(Breach.FromPwnBreach(remote));
                }
                else if (remote.ModifiedDate > local.ModifiedDate)
                {
                    await UpdateAsync(remote);
                    var index = localBreaches.FindIndex(b => b.Id == remote.Name);
                    localBreaches[index] = Breach.FromPwnBreach(remote);
                }
            }

            return localBreaches;
        }

        public async Task<Breach?> GetByIdAsync(string id)
        {
            var parameters = new Dictionary<string, object> { { "@Id", id } };
            var local = await _sqlService.QuerySingleAsync<Breach>(
                "SELECT * FROM Breaches WHERE Id = @Id", MapReaderToBreach, parameters);

            if (local != null) return local; 

            var remote = await _pwnApiService.GetBreachByNameAsync(id);
            
            await AddAsync(remote);
            return Breach.FromPwnBreach(remote);

        }


        private async Task AddAsync(PwnBreach breach)
        {
            var sql = @"INSERT INTO Breaches (
                Id, Title, Domain, BreachDate, AddedDate, ModifiedDate, PwnCount, 
                Description, LogoPath, Attribution, DisclosureUrl, DataClasses, 
                IsVerified, IsFabricated, IsSensitive, IsRetired, IsSpamList, 
                IsMalware, IsSubscriptionFree, IsStealerLog
            ) VALUES (
                @Id, @Title, @Domain, @BreachDate, @AddedDate, @ModifiedDate, @PwnCount, 
                @Description, @LogoPath, @Attribution, @DisclosureUrl, @DataClasses, 
                @IsVerified, @IsFabricated, @IsSensitive, @IsRetired, @IsSpamList, 
                @IsMalware, @IsSubscriptionFree, @IsStealerLog
            )";

            await _sqlService.ExecuteAsync(sql, GetParameters(breach));
        }

        private async Task UpdateAsync(PwnBreach breach)
        {
            var sql = @"UPDATE Breaches SET 
        Title = @Title, Domain = @Domain, BreachDate = @BreachDate, 
        AddedDate = @AddedDate, ModifiedDate = @ModifiedDate, PwnCount = @PwnCount, 
        Description = @Description, LogoPath = @LogoPath, Attribution = @Attribution, 
        DisclosureUrl = @DisclosureUrl, DataClasses = @DataClasses, 
        IsVerified = @IsVerified, IsFabricated = @IsFabricated, 
        IsSensitive = @IsSensitive, IsRetired = @IsRetired, 
        IsSpamList = @IsSpamList, IsMalware = @IsMalware, 
        IsSubscriptionFree = @IsSubscriptionFree, IsStealerLog = @IsStealerLog
        WHERE Id = @Id";

            await _sqlService.ExecuteAsync(sql, GetParameters(breach));
        }

        private Dictionary<string, object> GetParameters(PwnBreach b)
        {
            return new Dictionary<string, object>
            {
                { "@Id", b.Name },
                { "@Title", b.Title ?? (object)DBNull.Value },
                { "@Domain", b.Domain ?? (object)DBNull.Value },
                { "@BreachDate", b.BreachDate },
                { "@AddedDate", b.AddedDate },
                { "@ModifiedDate", b.ModifiedDate },
                { "@PwnCount", b.PwnCount },
                { "@Description", b.Description ?? (object)DBNull.Value },
                { "@LogoPath", b.LogoPath ?? (object)DBNull.Value },
                { "@Attribution", b.Attribution ?? (object)DBNull.Value },
                { "@DisclosureUrl", b.DisclosureUrl ?? (object)DBNull.Value },
                { "@DataClasses", b.DataClasses != null ? string.Join(",", b.DataClasses) : (object)DBNull.Value },
                { "@IsVerified", b.IsVerified ? 1 : 0 },
                { "@IsFabricated", b.IsFabricated ? 1 : 0 },
                { "@IsSensitive", b.IsSensitive ? 1 : 0 },
                { "@IsRetired", b.IsRetired ? 1 : 0 },
                { "@IsSpamList", b.IsSpamList ? 1 : 0 },
                { "@IsMalware", b.IsMalware ? 1 : 0 },
                { "@IsSubscriptionFree", b.IsSubscriptionFree ? 1 : 0 },
                { "@IsStealerLog", b.IsStealerLog ? 1 : 0 }
            };
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
