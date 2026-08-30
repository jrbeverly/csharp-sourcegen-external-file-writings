# Vision

Build a C# build-integrated metadata synthesis system capable of extracting structured semantic information from source code and emitting machine-readable artifacts automatically.

The goal is to transform the build process from purely binary compilation into a semantic infrastructure generation pipeline.

## Desired Workflow

Developers should annotate and structure code normally using:

- Attributes
- Interfaces
- Service clients
- Method annotations
- Capability metadata

The build system should then automatically:

- Analyze the source code
- Infer relationships
- Aggregate metadata
- Generate structured output artifacts

Without requiring developers to manually maintain separate infrastructure manifests.

## Example Permission Manifest Workflow

A developer may define service APIs such as:

- `ListThis`
- `GetThis`
- Other service operations

Annotated with required permissions.

As the application consumes those APIs, the build system should automatically:

- Discover usage
- Aggregate permissions
- Generate machine-readable manifests

The resulting artifact might state:

- Which permissions are required
- Which services are consumed
- Which capabilities are used

Based directly on code analysis.

## Generated Metadata Ecosystem

The architecture should support generation of artifacts including:

- Permission manifests
- Dependency manifests
- API interaction summaries
- Interface contracts
- Capability graphs
- Schema descriptions
- Service interaction specifications
- Behavioral metadata exports

The generated artifacts become part of the build outputs.

## Build-Time Semantic Extraction

The broader vision is to treat the build pipeline as a semantic extraction system.

Compilation should not merely produce binaries.

It should also produce:

- Operational metadata
- Infrastructure descriptions
- Capability summaries
- Dependency information
- Service interaction contracts

Derived directly from the source code itself.

## Tooling Integration

The architecture should integrate tightly with:

- Roslyn analyzers
- Source generators
- MSBuild
- Build pipelines
- Semantic analysis infrastructure

The process should ideally occur during compilation rather than requiring disconnected post-processing workflows.

## Conservative and Aggregate Modelling

The generated metadata does not need to perfectly model all runtime permutations.

For example:

- Applications may operate in multiple runtime modes.
- Permission requirements may vary dynamically.

The generated outputs may intentionally represent conservative aggregate capability sets where exact modeling is impractical.

## Long-Term Direction

The long-term goal is a build ecosystem where source code automatically produces not only executable binaries, but also rich semantic infrastructure artifacts.

In this model:

- Operational metadata becomes derived rather than manually maintained.
- Permissions become discoverable automatically.
- Capability usage becomes analyzable.
- Service interactions become machine-readable.
- Build outputs become semantically self-describing.

The resulting architecture turns compilation into a generalized metadata synthesis process driven directly from source code semantics and annotations.