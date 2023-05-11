using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadSingPDF.Infraestructure.DBSqlServer
{
    public class TransactSQL
    {
        private readonly Connection connectionObj;

        public TransactSQL()
        {
            if (connectionObj == null)
            {
                connectionObj = new Connection();
            }
        }

        public ResultSQL ExecuteQuery(string storeProcedure, params ParameterSQL[] parametersSQL)
        {
            if (connectionObj == null)
            {
                throw new Exception("Fallo conexion server database.");
            }

            DataSet dataset = new DataSet();

            try
            {
                using (SqlCommand command = PrepareSqlCommand(storeProcedure, connectionObj, parametersSQL))
                {
                    connectionObj.Open();
                    using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(command))
                    {
                        sqlAdapter.Fill(dataset);
                        sqlAdapter.Dispose();
                    }

                    command.Dispose();
                    connectionObj.Close();
                }
            }
            catch (Exception ex)
            {
                connectionObj.Close();
                throw ex;
            }

            return new ResultSQL(dataset);
        }


        private SqlCommand PrepareSqlCommand(string storeProcedure, Connection connection, params ParameterSQL[] parameters)
        {
            SqlCommand command = new SqlCommand(storeProcedure, connection.GetConnection())
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null && parameters.Length > 0)
            {
                parameters.ToList().ForEach(parameter =>
                {
                    command.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = $"@{parameter.Name}",
                        Value = parameter.Value,
                        SqlDbType = parameter.SqlDbType,
                        Size = parameter.Size,
                        Direction = parameter.Direction
                    });
                });
            }

            return command;
        }
    }
}
