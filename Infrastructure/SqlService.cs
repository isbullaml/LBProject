using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public class SqlService
    {
        private readonly string _connectionString;

        public SqlService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<List<T>> QueryAsync<T>(string sql, Func<SqlDataReader, T> map, Dictionary<string, object>? parameters = null)
        {
            var result = new List<T>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                if (parameters != null)
                {
                    foreach (var param in parameters)
                        command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(map(reader));
                    }
                }
            }

            return result;
        }

        public async Task<T?> QuerySingleAsync<T>(string sql, Func<SqlDataReader, T> map, Dictionary<string, object>? parameters = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                if (parameters != null)
                {
                    foreach (var param in parameters)
                        command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return map(reader);
                    }
                }
            }

            return default;
        }

        public async Task<int> ExecuteAsync(string sql, Dictionary<string, object>? parameters = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                if (parameters != null)
                {
                    foreach (var param in parameters)
                        command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }

                await connection.OpenAsync();
                return await command.ExecuteNonQueryAsync();
            }
        }
    }
}
