using System;
using NUnit.Framework;
using DoiTLean.Gzip;
namespace DoiTLean.Gzip.UnitTests;

public class GzipTests {

    /// <summary>
    /// Compress/Decompress round trip using the current (non-deprecated) API.
    /// </summary>
    [Test]
    public void CompressAndDecompress() {
        var txt = "GZipTest";
        var gzip = new Gzip();
        var Bin = gzip.GZip_StringCompress(txt);
        var newTxt = gzip.GZip_BinaryExpand(Bin);
        Assert.That(txt, Is.EqualTo(newTxt));
    }

    /// <summary>
    /// Empty string is a valid input distinct from null and must round-trip cleanly.
    /// </summary>
    [Test]
    public void CompressAndDecompress_EmptyString() {
        var gzip = new Gzip();
        var bin = gzip.GZip_StringCompress(string.Empty);
        var text = gzip.GZip_BinaryExpand(bin);
        Assert.That(text, Is.EqualTo(string.Empty));
    }

    [Test]
    public void StringCompress_NullInput_ThrowsArgumentException() {
        var gzip = new Gzip();
        Assert.Throws<ArgumentException>(() => gzip.GZip_StringCompress(null!));
    }

    [Test]
    public void BinaryExpand_NullInput_ThrowsArgumentException() {
        var gzip = new Gzip();
        Assert.Throws<ArgumentException>(() => gzip.GZip_BinaryExpand(null!));
    }

    [Test]
    public void BinaryExpand_MalformedInput_ThrowsArgumentException() {
        var gzip = new Gzip();
        var malformed = new byte[] { 1, 2, 3, 4, 5 };
        Assert.Throws<ArgumentException>(() => gzip.GZip_BinaryExpand(malformed));
    }

    /// <summary>
    /// GZip_BinayExpand is deprecated but must keep working exactly like GZip_BinaryExpand
    /// for existing OutSystems consumers that already reference it.
    /// </summary>
    [Test]
#pragma warning disable CS0618 // testing the deprecated wrapper on purpose
    public void DeprecatedBinayExpand_StillMatchesBinaryExpand() {
        var gzip = new Gzip();
        var bin = gzip.GZip_StringCompress("legacy consumer check");
        Assert.That(gzip.GZip_BinayExpand(bin), Is.EqualTo(gzip.GZip_BinaryExpand(bin)));
    }
#pragma warning restore CS0618

}
