# C# Build-Integrated Metadata Synthesis

Bridges the source-generator file boundary by lifting a generated permission-manifest constant into an external JSON artifact with MSBuild.

Generated `PermissionManifest.g.cs`:

```csharp
namespace PermissionManifest.Generated;

internal static class PermissionManifest
{
    public const string Json = "{\"permissions\":[\"s3:GetObject\",\"s3:ListBucket\",\"s3:PutObject\"]}";
}
```

`build/emit-manifest.targets` lifts the constant after the build:

```xml
<Target Name="EmitPermissionManifest" AfterTargets="Build">
  <ItemGroup>
    <PermissionManifestGenerated Include="$(CompilerGeneratedFilesOutputPath)/**/PermissionManifest.g.cs" />
  </ItemGroup>
  <LiftPermissionManifest GeneratedFile="@(PermissionManifestGenerated)"
                          ManifestFile="$(OutDir)permissions.json" />
</Target>
```

```sh
make build
make manifest
```

## Notes

- The source generator emits only C# source and never writes arbitrary files.
- An MSBuild inline task reads the generated source and writes `permissions.json` beside the assembly.
- Permission discovery is conservative, so statically visible calls remain in the manifest even behind runtime guards.
- Unchanged rebuilds reproduce identical manifest bytes.
