using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Signatures;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using ReadSingPDF.Infraestructure.DBSqlServer;
using ReadSingPDF.Models;
using ReadSingPDF.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReadSingPDF.Controllers
{
    public static class ProgramController
    {
        public static void PrepareCertificate(string pathPDF, string pathCertificate, DocumentPDF output, char[] passwordCertificate)
        {
            try
            {
                Pkcs12Store pk12 = new Pkcs12Store(new FileStream(pathCertificate, FileMode.Open, FileAccess.Read), passwordCertificate);
                string alias = null;

                foreach (object a in pk12.Aliases)
                {
                    alias = a.ToString();

                    if (pk12.IsKeyEntry(alias))
                        break;
                }

                ICipherParameters pk = pk12.GetKey(alias).Key;
                X509CertificateEntry[] ce = pk12.GetCertificateChain(alias);
                X509Certificate[] chain = new X509Certificate[ce.Length];

                for (int k = 0; k < ce.Length; ++k)
                    chain[k] = ce[k].Certificate;

                Sign(pathPDF, output.GetFullPath(), chain, pk, DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CMS);
                SaveData(chain, output);

                Console.WriteLine($"Archivo firmado con exito. Guardado en {output.GetFullPath()}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Algo ha ocurrido en el proceso. No se pudo firmar el archivo.\n\n***Error****: {0}", ex.Message));
            }
        }

        private static void SaveData(X509Certificate[] chain, DocumentPDF document)
        {
            UserPDF user = GetDataUser(chain);
            IDataSource dataSourceUser = new UserData();

            dataSourceUser.Save(user, document);
        }

        static UserPDF GetDataUser(X509Certificate[] chain)
        {
            if (chain.Length == 0)
                return new UserPDF()
                {
                    Email = string.Empty,
                    Name = string.Empty
                };


            X509Certificate element = chain[0];
            UserPDF user = new UserPDF()
            {
                Email = string.Empty,
                Name = string.Empty
            };

            var oid = element.SubjectDN.GetOidList();
            var values = element.SubjectDN.GetValueList();

            for (int i = 0; i < oid.Count; i++)
            {
                switch(oid[i].ToString())
                {
                    case "2.5.4.3":
                        user.Name = values[i].ToString();
                        break;

                    case "1.2.840.113549.1.9.1":
                        user.Email = values[i].ToString();
                        break;
                }
            }

            return user;
        }

        public static void Sign(string src, string dest, X509Certificate[] chain, ICipherParameters pk, string digestAlgorithm, PdfSigner.CryptoStandard subfilter)
        {
            PdfReader reader = new PdfReader(src);

            using (FileStream stream = new FileStream(dest, FileMode.Create))
            {
                PdfSigner signer = new PdfSigner(reader, stream, new StampingProperties());

                Rectangle rect = new Rectangle(36, 700, 500, 200);
                PdfSignatureAppearance appearance = signer.GetSignatureAppearance();
                appearance
                    .SetReason("Ejercicio práctico.")

                    .SetReuseAppearance(false)
                    .SetPageRect(rect)
                    .SetPageNumber(1);
                signer.SetFieldName("myFieldName");

                IExternalSignature pks = new PrivateKeySignature(pk, digestAlgorithm);

                signer.SignDetached(pks, chain, null, null, null, 0, subfilter);

                reader.Close();
            }
        }

        public static string OpenFileDialog(string filterExt)
        {
            if (string.IsNullOrEmpty(filterExt))
                return string.Empty;

            if (filterExt.Replace(" ", "") == string.Empty)
                return string.Empty;

            OpenFileDialog openFileDialog1 = new OpenFileDialog()
            {
                Title = string.Format("Seleccionar un archivo {0}", filterExt.ToUpper()),

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = filterExt,
                Filter = $"Archivos (*.{filterExt})|*.{filterExt}"
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                return openFileDialog1.FileName;

            return string.Empty;
        }

        public static string SelectFiles(string ext)
        {
            string dialog = string.Empty;

            Console.Write($"Seleccionar un archivo .{ext} (y/Y): ");

            if (Console.ReadLine().ToLower() != "y")
                return SelectFiles(ext);

            dialog = OpenFileDialog(ext);

            if (string.IsNullOrEmpty(dialog))
                return SelectFiles(ext);

            Console.WriteLine($"Archivo seleccionado: {dialog}.\n");

            return dialog;
        }

        public static string SelectOutputFolder(string pathPDF)
        {
            string folder = string.Empty;

            Console.Write($"\nSeleccionar una carpeta de ubicacion (y/Y): ");

            if (Console.ReadLine().ToLower() != "y")
                return SelectOutputFolder(pathPDF);

            folder = OpenFolderDialog();

            Console.WriteLine($"Ubicacion seleccionada: {folder}\n");

            return folder;
        }

        private static string OpenFolderDialog()
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrEmpty(folderDialog.SelectedPath))
                    return folderDialog.SelectedPath;
            }

            return OpenFolderDialog();
        }

        public static string GetPasswod()
        {
            Console.Write($"Ingresar clave del certificado (si no tiene teclear enter): ");

            return Console.ReadLine();
        }

        public static string GenerateRandomFileName(string filename)
        {
            return string.Format("{0}_{1}", DateTime.Now.Millisecond.ToString().Replace(":", ""), System.IO.Path.GetFileName(filename));
        }
    }
}
