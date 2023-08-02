using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using OutSystems.ExternalLibraries.SDK;

namespace DoiTLean.Gzip
{

    /// <summary>
    /// With the arrival of Outsystems solutions to other devices than computers served by cable networks, that is, 
    /// their entry into situations supported by mobile networks, and low-end devices like smart phones, 
    /// there is a need to implement solutions to culminate some inherent difficulties in the use of mobile networks.
    /// This solution compresses the information to pass through the mobile network in order to reduce its weight,
    /// improving efficiency.It also decompresses the information so that it can be used outside modern browsers.
    /// </summary>
    [OSInterface(Description = "Gzip Compress/Uncompress", IconResourceName = "DoiTLean.Gzip.resources.gzip.png", Name = "Gzip")]
    public interface IGzip
    {

        /// <summary>
        /// Compress a string and return the respective binary.
        /// </summary>
        /// <param name="InText"></param>
        [OSAction(Description = "Compress a string and return the respective binary.", IconResourceName = "DoiTLean.Gzip.resources.gzip.png", ReturnName = "OutBinary")]
        byte[] GZip_StringCompress(
            string InText);


        /// <summary>
        /// Binary expand method
        /// </summary>
        /// <param name="InBinary"></param>
        [OSAction(Description = "Binary expand method", IconResourceName = "DoiTLean.Gzip.resources.gzip.png", ReturnName = "OutText")]
        string GZip_BinayExpand(
            byte[] InBinary);
    }

}