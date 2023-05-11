using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadSingPDF.Infraestructure.DBSqlServer
{
    public class ParameterSQL
    {
        public ParameterSQL(string name, object value, SqlDbType dbType, int size = 0, ParameterDirection direction = ParameterDirection.Input)
        {
            Name = name;
            Value = value ?? DBNull.Value;
            SqlDbType = dbType;
            Size = size;
            Direction = direction;
        }

        public string Name { get; set; }
        public object Value { get; set; }
        public SqlDbType SqlDbType { get; set; }
        public int Size { get; set; }
        public ParameterDirection Direction { get; set; }
    }
}
