# Problem

Modern codebases contain a large amount of implicit operational metadata that is never formally extracted into structured machine-readable artifacts.

Examples include:

- API permissions
- Dependency relationships
- Interface usage
- Service interaction patterns
- Capability usage
- External system integrations
- Behavioral metadata
- Contract semantics

In many cases, this information already exists inside the codebase through:

- Attributes
- Annotations
- Type relationships
- Method calls
- Interface implementations
- Source structure
- Dependency usage patterns

However, this metadata is rarely systematically extracted during the build process.

## Core Problem

The build process typically produces binaries and traditional compiled artifacts but does not comprehensively produce structured metadata describing:

- What the application consumes
- What capabilities it requires
- What APIs it interacts with
- What permissions it needs
- What dependencies it relies upon

As a result:

- Operational metadata becomes fragmented
- Permission requirements become manually maintained
- Dependency understanding becomes incomplete
- Service contracts become implicit
- Build artifacts lack semantic infrastructure descriptions

The problem is therefore:

How can build-time code analysis systematically extract structured metadata from a codebase and emit machine-readable artifacts automatically?

## Example Scenario

A service client API may contain methods such as:

- `ListThis`
- `GetThis`
- Other service operations

Those methods may be annotated with metadata describing required permissions.

The system should be able to:

- Analyze the codebase
- Discover which APIs are consumed
- Aggregate required permissions
- Generate a structured output artifact

For example:

- JSON permission manifests
- YAML capability descriptions
- API interaction summaries

## Requirements

The system should support generation of metadata artifacts including:

- Permission manifests
- Dependency manifests
- API usage summaries
- Interface contracts
- Schema descriptions
- Capability graphs
- Service interaction specifications
- Other structured metadata derivable from source code

## Build Integration Constraints

The metadata generation process should occur automatically as part of the build itself.

Preferred approaches include:

- Source generators
- Roslyn analyzers
- Compile-time analysis
- MSBuild integration

Fallback approaches may include:

- Post-build tooling
- Supplemental analysis tasks

The goal is to minimize separation between compilation and metadata generation.

## Imperfect Semantic Modelling

The generated metadata does not necessarily need to model every possible runtime behavior variation.

For example:

- A single application may operate in multiple modes with different permission requirements.
- Static analysis may only produce aggregate or conservative permission sets.

The system should still provide useful derived metadata despite those limitations.

## Success Criteria

The system succeeds if it can:

- Analyze source code automatically during builds
- Extract meaningful structured metadata
- Generate machine-readable artifacts
- Aggregate semantic information from annotations and source structure
- Integrate naturally into the build pipeline
- Reduce manually maintained operational metadata
- Produce useful infrastructure and capability descriptions from code itself