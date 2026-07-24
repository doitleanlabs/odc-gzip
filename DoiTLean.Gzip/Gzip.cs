using System;
using System.IO;
using System.IO.Compression;

namespace DoiTLean.Gzip
{
    public class Gzip : IGzip
    {

        public byte[] GZip_StringCompress(string InText)
        {
            if (InText is null)
            {
                throw new ArgumentException("InText must not be null.", nameof(InText));
            }

            var buffer = System.Text.Encoding.UTF8.GetBytes(InText);

            // GZipStream must be disposed (flushes the trailer) before reading the memory stream back out,
            // so the write and the ToArray() cannot share the same using block.
            using var oMemoryStream = new MemoryStream();
            using (var compressedzipStream = new GZipStream(oMemoryStream, CompressionMode.Compress, true))
            {
                compressedzipStream.Write(buffer, 0, buffer.Length);
            }

            return oMemoryStream.ToArray();
        }// StringCompress


        /// <summary>
        /// Deprecated: kept only for backward compatibility with existing OutSystems consumers that already
        /// reference this action name. New consumers should use <see cref="GZip_BinaryExpand"/>.
        /// </summary>
        [Obsolete("Use GZip_BinaryExpand instead. Kept for backward compatibility with existing OutSystems consumers.")]
        public string GZip_BinayExpand(byte[] InBinary)
        {
            return GZip_BinaryExpand(InBinary);
        }// BinayExpand (deprecated wrapper, kept for compatibility)


        public string GZip_BinaryExpand(byte[] InBinary)
        {
            if (InBinary is null)
            {
                throw new ArgumentException("InBinary must not be null.", nameof(InBinary));
            }

            try
            {
                using var inputStream = new MemoryStream(InBinary);
                using var gZipStream = new GZipStream(inputStream, CompressionMode.Decompress);
                using var streamReader = new StreamReader(gZipStream);
                return streamReader.ReadToEnd();
            }
            catch (InvalidDataException ex)
            {
                throw new ArgumentException("InBinary is not a valid gzip-compressed payload.", nameof(InBinary), ex);
            }
        }// BinaryExpand

    }
}
