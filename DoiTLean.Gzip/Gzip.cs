using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace DoiTLean.Gzip
{
    public class Gzip : IGzip
    {

        public byte[] GZip_StringCompress(string InText)
        {
            byte[] OutBinary;
            byte[] buffer = null;
            byte[] compressedData = null;
            MemoryStream oMemoryStream = null;
            System.IO.Compression.GZipStream compressedzipStream = null;
            oMemoryStream = new MemoryStream();
            buffer = System.Text.Encoding.UTF8.GetBytes(InText);
            compressedzipStream = new System.IO.Compression.GZipStream(oMemoryStream, System.IO.Compression.CompressionMode.Compress, true);
            compressedzipStream.Write(buffer, 0, buffer.Length);
            compressedzipStream.Dispose();
            compressedzipStream.Close();
            compressedData = oMemoryStream.ToArray();
            oMemoryStream.Close();
            OutBinary = compressedData;
            return OutBinary;

        }// StringCompress


        public string GZip_BinayExpand(byte[] InBinary)
        {
            string OutText = "";
            byte[] inputBytes = InBinary;

            using (var inputStream = new MemoryStream(inputBytes))
            using (var gZipStream = new GZipStream(inputStream, CompressionMode.Decompress))
            using (var streamReader = new StreamReader(gZipStream))
            {
                OutText = streamReader.ReadToEnd();

            }// Output
            return OutText;
        }// BinayExpan


    }
}
