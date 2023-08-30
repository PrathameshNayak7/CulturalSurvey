using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alert.Class
{
    public class DBLayer
    {
        public async Task<int> ExecuteAsync(string sConnectionString,string sSQL, params MySqlParameter[] parameters)
        {
            using (var newConnection = new MySqlConnection(sConnectionString))
            {
                using (var newCommand = new MySqlCommand(sSQL, newConnection))
                {
                    newCommand.CommandType = CommandType.StoredProcedure;
                    if (parameters != null) newCommand.Parameters.AddRange(parameters);

                    await newConnection.OpenAsync().ConfigureAwait(false);
                    return await newCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            }
        }

        public async Task<DataTable> ExecuteAdapterAsync(int iText,string sConnectionString,string sSql, params MySqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            if (iText == 0)
            {
                using (var newConnection = new MySqlConnection(sConnectionString))
                {
                    using (var newCommand = new MySqlCommand(sSql, newConnection))
                    {
                        newCommand.CommandType = CommandType.StoredProcedure;
                        if (parameters != null) newCommand.Parameters.AddRange(parameters);

                        await newConnection.OpenAsync().ConfigureAwait(false);
                        var reader = await newCommand.ExecuteReaderAsync();
                        dt.Load(reader);

                        return dt;
                    }
                }
            }
            else
            {
                using (var newConnection = new MySqlConnection(sConnectionString))
                {
                    using (var newCommand = new MySqlCommand(sSql, newConnection))
                    {
                        newCommand.CommandType = CommandType.Text;
                        if (parameters != null) newCommand.Parameters.AddRange(parameters);

                        await newConnection.OpenAsync().ConfigureAwait(false);
                        var reader = await newCommand.ExecuteReaderAsync();
                        dt.Load(reader);

                        return dt;
                    }
                }
            }
        }

        public async Task<DataTable> GetDataDatatable(string sConnectionString, string sSql, params MySqlParameter[] parameters)
        {
            var dt = new DataTable();
            using (var connection = new MySqlConnection(sConnectionString))
            {
                using (var newCommand = new MySqlCommand(sSql, connection))
                {
                    await connection.OpenAsync().ConfigureAwait(false);
                    if (parameters != null)
                        newCommand.Parameters.AddRange(parameters);
                    var reader = await connection.CreateCommand().ExecuteReaderAsync();

                    dt.Load(reader);

                    return dt;
                }
            }
        }

        public async Task<string> ExecuteAsyncWithOutPara(string sConnectionString, string sSQL, params MySqlParameter[] parameters)
        {            
            using (var newConnection = new MySqlConnection(sConnectionString))
            {
                using (var newCommand = new MySqlCommand(sSQL, newConnection))
                {
                    newCommand.CommandType = CommandType.StoredProcedure;
                    if (parameters != null) newCommand.Parameters.AddRange(parameters);
                    newCommand.Parameters.Add("p_Status", MySqlDbType.VarChar, 500);
                    newCommand.Parameters["p_Status"].Direction = ParameterDirection.Output;

                    await newConnection.OpenAsync().ConfigureAwait(false);
                    await newCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                    return (string)newCommand.Parameters["p_Status"].Value;
                }
            }
        }
    }

}
