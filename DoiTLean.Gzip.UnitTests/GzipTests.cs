using NUnit.Framework;
using DoiTLean.Gzip;
namespace DoiTLean.Gzip.UnitTests;

public class GzipTests {

    /// <summary>
    /// Compress/Decompress
    /// </summary>
    [Test]
    public void CompressAndDecompress() {
        var txt = "GZipTest";
        var gzip = new Gzip();
        var Bin = gzip.GZip_StringCompress(txt);
        var newTxt = gzip.GZip_BinayExpand(Bin);
        Assert.That(txt, Is.EqualTo(newTxt));
    }


}