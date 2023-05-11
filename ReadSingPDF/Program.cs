using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Signatures;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using ReadSingPDF.Controllers;
using ReadSingPDF.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReadSingPDF
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("=== FIRMADOR PDF ===\n");
            Console.WriteLine("** Para realizar una accion ingrese y o Y **\n");

            string pathPDF = ProgramController.SelectFiles("pdf");
            string pathCertificate = ProgramController.SelectFiles("pfx");
            char[] passwordCertificate = ProgramController.GetPasswod().ToCharArray();
            DocumentPDF output = new DocumentPDF()
            {
                Path = ProgramController.SelectOutputFolder(pathPDF),
                Name = ProgramController.GenerateRandomFileName(pathPDF)
            };

            ProgramController.PrepareCertificate(pathPDF, pathCertificate, output, passwordCertificate);
            
            Console.ReadKey();
        }
    }
}
