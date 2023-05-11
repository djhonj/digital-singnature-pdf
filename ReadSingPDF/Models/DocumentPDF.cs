using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadSingPDF.Models
{
    public class DocumentPDF
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public string GetFullPath() => Path + "\\" + Name;
    }
}
