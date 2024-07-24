using Azure;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Unifyology.Encryption.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var regex = new Regex(@"(.*):(.*)at(.*)", RegexOptions.IgnoreCase);
            var matches = regex.Match("Test Error: System.Exception:        at Test.System.Exception.WorkUnit");
            var addresses = Regex.Split("Hauptstraße 5 10178 Berlin", "[0-9]{5}", RegexOptions.IgnoreCase);
            var match = Regex.Match("Hauptstraße 5 10178 Berlin", "[0-9]{5}", RegexOptions.IgnoreCase);
            ExtractCode("240320_AHW779_004_055_324562_ABGEMELDET");
            var fileName = GenerateScanCodeForCustomer261510("240320_AHW779_004_055_324562_ABGEMELDET", 1);

            Console.WriteLine(match.Value);
            //using (Aes aesAlgorithm = Aes.Create())
            //{
            //    aesAlgorithm.KeySize = 256;
            //    aesAlgorithm.GenerateKey();
            //    string keyBase64 = Convert.ToBase64String(aesAlgorithm.Key);
            //    Console.WriteLine($"Aes Key Size : {aesAlgorithm.KeySize}");
            //    Console.WriteLine("Here is the Aes key in Base64:");
            //    Console.WriteLine(keyBase64);
            //}
            //var s = "Volvo@123";
            //var s1 = Rot128("\u0016/,6/@qrs");
            //var converted = Encode(s);
            //var decode = Decode(converted);
            //Console.WriteLine(converted);
            //XmlDocument xml = new XmlDocument();
            //xml.LoadXml(@"<Array id=""Parameters"" xmlns=""ng:eie:common:datatypes""><Struct id=""ParameterSet1""><UTF8String id=""Location"">S810</UTF8String></Struct></Array>"); // suppose that myXmlString contains "<Names>...</Names>"
            //var node = xml.ChildNodes[0];
            //var xnList = xml.SelectNodes("/Array/Struct/UTF8String");
            //var newElem = xml.CreateNode(XmlNodeType.Element, "Quantity", string.Empty);
            //var elem = xml.CreateElement("Quantity");
            UploadFile();
        }

        string Test(string s = "")
        {
            return s;
        }

        static string Rot128(string strText)
        {
            string strOutput = String.Empty;
            foreach (char charValue in strText)
            {
                int intValue = charValue;
                if (intValue < 64)
                {
                    strOutput += Convert.ToChar(intValue + 64);
                }
                else if (intValue > 64)
                {
                    strOutput += Convert.ToChar(intValue - 64);
                }
                else
                {
                    strOutput += charValue;
                }
            }
            return strOutput;
        }

        /// <summary>
        /// Check if string is Base64
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static bool IsBase64String(string base64)
        {
            try
            {
                return Convert.FromBase64String(base64).Length > 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Convert string to Base64
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static string ToBase64String(string value)
        {
            if (IsBase64String(value)) return value;
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// Convert string to Base64
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static string FromBase64String(string base64)
        {
            if (!IsBase64String(base64)) return base64;
            return Encoding.UTF8.GetString(Convert.FromBase64String(base64));
        }

        /// <summary>
        /// encode a string based on base64
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Encode(string value)
        {
            return ToBase64String(Rot128(value));
        }

        /// <summary>
        /// decode a string from base64
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static string Decode(string base64)
        {
            return Rot128(FromBase64String(base64));
        }

        public static void UploadFile()
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=portfoliosa;AccountKey=J17BQk5ExqLHPBaEYkEyMq7o12V6PTGF7VJXSuyOSUw6Xze0OkIHvLdfeY3LOQ9n+gPrjLqe6mpQ0sncBrwRFw==;EndpointSuffix=core.windows.net";

            // Name of the share, directory, and file we'll create
            string shareName = "sample-share";
            string dirName = "sample-dir";
            string fileName = "sample-file";

            // Path to the local file to upload
            string localFilePath = @"D:\CRM2013-Bids-ENU-i386.exe";
            fileName = Path.GetFileName(localFilePath);
            // Get a reference to a share and then create it
            ShareClient share = new ShareClient(connectionString, shareName);
            // share.Create();

            // Get a reference to a directory and create it
            ShareDirectoryClient directory = share.GetDirectoryClient(dirName);
            //directory.Create();

            // Get a reference to a file and upload it
            ShareFileClient file = directory.GetFileClient(fileName);
            using (FileStream stream = File.OpenRead(localFilePath))
            {
                UploadFile(directory, fileName, stream);
            }
        }

        public static void UploadFile(ShareDirectoryClient directory, string filename, Stream stream)
        {
            int blockSize = 3 * 1024 * 1024;
            stream.Seek(0, SeekOrigin.Begin);
            var fileClient = directory.GetFileClient(filename);
            fileClient.Create(stream.Length);
            if (stream.Length <= blockSize)
            {
                fileClient.UploadRange(new HttpRange(0, stream.Length), stream);
                return;
            }

            byte[] buffer = new byte[blockSize];
            int uploadingBytes;
            long start = 0;

            while ((uploadingBytes = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                using MemoryStream ms = new(buffer, 0, uploadingBytes);
                fileClient.UploadRange(ShareFileRangeWriteType.Update, new HttpRange(start, ms.Length), ms);
                start += ms.Length;
            }
        }

        private static string GenerateScanCodeForCustomer261510(string fileName, int docCount)
        {
            Regex regex = new("(\\d{6})_?([a-zA-Z]+\\d+)_(\\d+)_(\\d+)_(\\d+)(_?[NEUFAHRZEUG|ZUGELASSEN|ABGEMELDET]+)", RegexOptions.IgnoreCase);
            var matches = regex.Matches(fileName);
            if (matches.Count != 1)
            {
                return string.Empty;
            }

            var match = matches[0];

            return $"{match.Groups[1].Value}_{match.Groups[2].Value}_{match.Groups[3].Value}_{match.Groups[4].Value}_{docCount}_{match.Groups[5].Value}{match.Groups[6].Value}";
        }

        private static void ExtractCode(string fileName)
        {
            Regex regex = new("(.*)_([NEUFAHRZEUG|ZUGELASSEN|ABGEMELDET]+)$", RegexOptions.IgnoreCase);
            var matches = regex.Matches(fileName);
            
        }
    }
}