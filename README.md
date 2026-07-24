# odc-gzip

Source code for an OutSystems Developer Cloud (ODC) External Library that exposes gzip
compress/decompress utilities as an `OSInterface` external logic module.

For OutSystems-side usage (installing the library, action inputs/outputs, JSON examples,
error handling), see [DOC.md](DOC.md). This file covers building and maintaining the code.

## Requirements

- .NET SDK 10.0 or later ([download](https://dotnet.microsoft.com/download))
- (Packaging only) PowerShell (`pwsh`), to run `generate_upload_package.ps1`

## Repository structure

```
DoiTLean.Gzip/                     Library project (the External Library itself)
  DoiTLean.Gzip.sln                Solution file
  DoiTLean.Gzip.csproj
  IGzip.cs                         OSInterface contract (public actions exposed to OutSystems)
  Gzip.cs                          Implementation
  resources/gzip.png               Icon embedded as a resource, referenced by the OSAction attributes
  generate_upload_package.ps1      Publishes + zips the library for upload to ODC
DoiTLean.Gzip.UnitTests/           NUnit test project
  GzipTests.cs
Dist/                              Output of the packaging script (ODC-Gzip.zip); not committed
```

## Build

```bash
dotnet build DoiTLean.Gzip/DoiTLean.Gzip.sln -c Release
```

## Test

```bash
dotnet test DoiTLean.Gzip/DoiTLean.Gzip.sln -c Release
```

## Package for upload to ODC

From `DoiTLean.Gzip/`, with PowerShell available:

```powershell
./generate_upload_package.ps1
```

This runs `dotnet publish -c Release -r linux-x64 --self-contained false` and zips the publish
output into `../Dist/ODC-Gzip.zip`, which is what you upload as the External Library asset in
ODC Studio / OutSystems.

## Core concepts

- **`IGzip`** is the `OSInterface` — its public methods, decorated with `[OSAction]`, are exactly
  the actions OutSystems developers see and call. Method names, signatures, and return names are
  a public contract: renaming or changing them breaks existing OutSystems apps that consume this
  library.
- **`Gzip`** implements `IGzip` using `System.IO.Compression.GZipStream`.
- Public actions validate their inputs (reject `null` with a clear `ArgumentException`) and
  translate low-level decompression failures (malformed gzip payloads) into a clear
  `ArgumentException` instead of letting a raw `InvalidDataException` cross into OutSystems.
- `GZip_BinayExpand` (note the typo in the name) is kept only for backward compatibility with
  OutSystems apps that already reference it; it now delegates to the correctly-spelled
  `GZip_BinaryExpand`, which is the one new consumers should use. See [DOC.md](DOC.md) for the
  full action reference.

## Known limitations

- `GZip_BinayExpand` is deprecated (misspelled name, kept for compatibility) — do not build new
  logic against it.
- The packaging script (`generate_upload_package.ps1`) targets `linux-x64`; if ODC ever needs a
  different runtime identifier, update the `-r` flag and the corresponding path inside the script.
- No compression-level or format options are exposed (always standard gzip, default compression
  level).

## OutSystems Links

- [ODC Documentation](https://success.outsystems.com/documentation/outsystems_developer_cloud/)
- [ODC External Logic](https://success.outsystems.com/documentation/outsystems_developer_cloud/building_apps/extend_your_apps_with_external_logic/)
- [ODC External Libraries SDK](https://success.outsystems.com/documentation/outsystems_developer_cloud/building_apps/extend_your_apps_with_external_logic/external_libraries_sdk_readme/)
