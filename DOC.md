# Gzip External Library — OutSystems ODC Guide

This document is for OutSystems developers consuming the **Gzip** External Library in an
OutSystems Developer Cloud (ODC) app. It describes what to install, the actions available, and
how to handle errors. For the library's own source code, see [README.md](README.md).

## Installing the library

1. In ODC Portal, go to your application's dependencies (or Workspace-level integrations) and
   add the **Gzip** External Library.
   - If your organization builds this library from source, ask a maintainer to run the packaging
     step (see README.md) and upload the resulting `ODC-Gzip.zip` as a new External Library
     version.
2. Add the library as a dependency of your OutSystems module.
3. The **Gzip** interface becomes available in the module's Interface tab, under the library's
   name, with two actions described below.

## Actions

### `GZip_StringCompress`

Compresses a text value and returns the compressed binary (gzip format).

| Parameter | Direction | OutSystems type | Notes |
|-----------|-----------|------------------|-------|
| `InText`  | Input     | Text             | Must not be null. Empty text (`""`) is valid and compresses to a small non-empty binary. |
| `OutBinary` | Output  | Binary Data      | Gzip-compressed bytes. |

**Example**

Input:
```json
{ "InText": "Hello OutSystems!" }
```

Output (binary, shown as base64 for illustration):
```json
{ "OutBinary": "H4sIAAAAAAAAC/NIzcnJVyjPL8pJ4QIAlyDA3RIAAAA=" }
```

**Errors**

- If `InText` is null (not applicable when the input is bound to a Text variable, which defaults
  to `""`, but relevant if built dynamically), the action throws an exception with a clear message
  (`InText must not be null.`). Wrap the call in an Exception Handler if `InText` can come from an
  untrusted or optional source.

### `GZip_BinaryExpand`

Decompresses a gzip-compressed binary back into the original text. This is the current,
correctly-named action — use this one for all new development.

| Parameter | Direction | OutSystems type | Notes |
|-----------|-----------|------------------|-------|
| `InBinary` | Input    | Binary Data      | Must be a valid gzip payload (e.g. produced by `GZip_StringCompress`). Must not be null. |
| `OutText`  | Output   | Text             | Decompressed text. |

**Example**

Input:
```json
{ "InBinary": "H4sIAAAAAAAAC/NIzcnJVyjPL8pJ4QIAlyDA3RIAAAA=" }
```

Output:
```json
{ "OutText": "Hello OutSystems!" }
```

**Errors**

- If `InBinary` is null, the action throws with message `InBinary must not be null.`.
- If `InBinary` is not a valid gzip payload (corrupted, wrong format, or truncated), the action
  throws with message `InBinary is not a valid gzip-compressed payload.`. Always validate or
  wrap this call in an Exception Handler when the binary comes from an external source (upload,
  API response, etc.) rather than directly from `GZip_StringCompress`.

### `GZip_BinayExpand` (deprecated — do not use in new logic)

Identical behavior to `GZip_BinaryExpand` (it delegates to it internally). Kept only so
OutSystems apps built before this action was renamed keep working unchanged. New logic must call
`GZip_BinaryExpand` instead — this action may be removed in a future major version.

| Parameter | Direction | OutSystems type | Notes |
|-----------|-----------|------------------|-------|
| `InBinary` | Input    | Binary Data      | Same rules as `GZip_BinaryExpand`. |
| `OutText`  | Output   | Text             | Same as `GZip_BinaryExpand`. |

## Handling errors in OutSystems

All three actions can throw exceptions on invalid input (see per-action tables above). In
OutSystems, wrap calls in an **Exception Handler** node and branch on the exception message, or
validate inputs before calling (e.g. check `InBinary` is not empty/null before calling
`GZip_BinaryExpand`) if you need a specific user-facing error instead of a generic failure.

## Known limitations

- Only the standard gzip format is supported (no configurable compression level, no other
  archive formats).
- Text is always encoded/decoded as UTF-8.
