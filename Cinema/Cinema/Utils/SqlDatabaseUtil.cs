using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using AutoMapper;
using Cinema.Models.Domain;

namespace Cinema.Utils
{
    public class SqlDatabaseUtil
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;

        public SqlDatabaseUtil(IMapper mapper)
        {
            _mapper = mapper;
            _connectionString = ConfigurationManager.ConnectionStrings["Cinema"].ConnectionString;
        }

        public IEnumerable<T> Select<T>(string sql, params SqlParameter[] parameters)
        {
            var results = new List<T>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new SqlCommand(sql, connection);
                if (parameters != null && parameters.Any())
                {
                    cmd.Parameters.AddRange(parameters);
                }
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            results.Add(_mapper.Map<T>(reader));
                        }
                    }
                }
            }

            return results;
        }

        public bool Execute(string sql, params SqlParameter[] parameters)
        {
            int result;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = new SqlCommand(sql, connection);
                if (parameters != null && parameters.Any())
                {
                    cmd.Parameters.AddRange(parameters);
                }

                result = cmd.ExecuteNonQuery();
            }

            return result != 0;
        }

        public IEnumerable<T> Execute<T>(string storedProcedureName, SqlParameter[] parameters = null)
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;
            var results = new List<T>();

            try
            {
                connection = new SqlConnection(_connectionString);
                connection.Open();

                SqlCommand cmd = new SqlCommand(storedProcedureName, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null && parameters.Any())
                {
                    cmd.Parameters.AddRange(parameters);
                }

                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        results.Add(Mapper.Map<T>(reader));
                    }
                }
            }
            finally
            {
                connection?.Close();
                reader?.Close();
            }

            return results;
        }
    }
}