using ReadSingPDF.Models;
using ReadSingPDF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ReadSingPDF.Infraestructure.DBSqlServer
{
    public class UserData : IDataSource
    {
        private readonly TransactSQL _transactSQL;

        public UserData()
        {
            _transactSQL = new TransactSQL();
        }

        public void Save(UserPDF user, DocumentPDF document)
        {
            _transactSQL.ExecuteQuery("SaveData", new ParameterSQL("name", user.Name, SqlDbType.VarChar, 200, ParameterDirection.Input)
                                                , new ParameterSQL("email", user.Email, SqlDbType.VarChar, 300, ParameterDirection.Input)
                                                , new ParameterSQL("document", document.Name, SqlDbType.VarChar, -1, ParameterDirection.Input)
                                                , new ParameterSQL("path", document.Path, SqlDbType.VarChar, -1, ParameterDirection.Input)
            );
        }
    }
}
