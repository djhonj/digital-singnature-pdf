using ReadSingPDF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadSingPDF.Services
{
    interface IDataSource
    {
        void Save(UserPDF user, DocumentPDF document);
    }
}
