# Mock Design

## Purpose

This document evaluates the concept described in `PROBLEM.md`, `TECHNICAL.md`, and `VISION.md` and proposes a high-level design direction.

It is intentionally conceptual. The goal is to determine whether the proposed system is coherent and implementable, not to lock in detailed schemas, exact file formats, or low-level implementation choices.

## Executive Assessment

The proposed system is fundamentally implementable.

The concept becomes technically credible if it is treated as a **build-integrated semantic extraction pipeline** built from a combination of:

- Roslyn-based source analysis for discovery and validation
- Shared metadata extraction logic
- MSBuild-driven artifact emission

The key architectural conclusion is:

- **Semantic analysis belongs close to Roslyn**
- **External artifact emission belongs under build orchestration**

That means source generators may be useful, but they should not be the only or primary mechanism for producing authoritative external metadata artifacts.

## Core Design Position

The repository describes a system that turns source code into two classes of outputs:

1. Traditional build outputs such as assemblies
2. Derived machine-readable metadata describing operational semantics

This is a coherent direction.

The most realistic interpretation is not "infer everything from arbitrary code," but:

- developers expose meaningful metadata through attributes, interfaces, and other source-level declarations
- the build pipeline statically analyzes those declarations and usage patterns
- the system produces conservative, structured summaries that are useful even when they are not perfect runtime models

That interpretation is both feasible and aligned with the documents.

## Recommended Scope Framing

The current written vision is broad. It spans permissions, dependencies, API usage, interface contracts, capability graphs, schemas, and behavioral metadata.

That breadth is useful as a long-term direction, but too broad for a first architectural commitment.

The design should treat the platform as an **extensible metadata synthesis framework** with a narrow initial proving ground.

Recommended initial proving ground:

- permission manifests
- API usage summaries
- service interaction summaries

Why this is a good first slice:

- the example scenario already centers on permissions
- method-level annotations and call-site discovery are relatively analyzable
- the output is easy to evaluate for usefulness
- it exercises the core architecture without requiring full-program semantic modeling

## Viability

### What is conceptually strong

The following are good fits for build-time static analysis:

- attributes and annotations attached to symbols
- interface implementations and type relationships
- direct method invocations and symbol references
- project and assembly dependency structure
- explicit capability declarations in code

These are all available to compiler-driven analysis and can be turned into structured metadata with good reliability.

### What is conceptually weaker

The following are harder to model accurately with static analysis alone:

- reflection-based behavior
- dynamic loading or plugin discovery
- configuration-driven service selection
- permissions that vary heavily by runtime mode
- behavior hidden behind generic abstractions or indirection layers

These do not make the system non-viable, but they force an important product decision:

- the system must prefer **conservative, explainable outputs**
- the system must avoid claiming complete runtime truth

### Viability Conclusion

The system is viable if the product definition accepts that:

- outputs are derived from statically visible evidence
- some metadata will be aggregate or over-approximated
- explicit annotations are part of the authoring model

The system is not viable if success depends on precise reconstruction of arbitrary runtime behavior without additional declarations.

## Proposed High-Level Architecture

### 1. Source Metadata Surface

Application and library code remain the authoring surface.

Developers describe semantics through mechanisms such as:

- attributes on APIs, types, or members
- marker interfaces
- structured conventions
- explicit capability declarations

This layer is important because the system needs stable semantic anchors. Pure inference alone will not be sufficient for many metadata kinds.

### 2. Semantic Extraction Core

A shared analysis core should own the logic for:

- walking syntax and symbols
- resolving semantic relationships
- recognizing supported metadata patterns
- producing a normalized internal metadata graph or fact set

This core should be independent from any single hosting mechanism so that it can be reused by:

- build-time analyzers
- source generators if needed
- MSBuild tasks
- test harnesses

That separation keeps the architecture coherent and reduces duplicated analysis logic.

### 3. Analyzer and Validation Layer

Roslyn analyzers should provide fast feedback to developers during normal development and builds.

Their role is not mainly artifact generation. Their role is to:

- validate supported annotation patterns
- report missing or contradictory metadata
- flag unsupported constructs when they affect extraction quality
- explain why a manifest may be incomplete or conservative

This improves trust in the generated outputs.

### 4. Optional Generator Layer

Source generators may still be useful, but only in targeted roles such as:

- generating helper code tied to discovered metadata
- producing compile-visible glue code
- surfacing developer-friendly views of extracted declarations

They should be optional to the core metadata artifact pipeline, not the sole delivery mechanism for external manifests.

### 5. Build Emission Layer

An MSBuild-integrated emission step should own final artifact production.

Its responsibilities are:

- invoke or host the extraction core in a build-safe way
- determine which artifacts are enabled for the current project
- write machine-readable outputs to build locations
- include selected artifacts alongside normal outputs

This layer is the right place to manage output files, build paths, and packaging concerns.

### 6. Artifact Emitters

Artifact emitters translate the normalized metadata into external representations such as:

- JSON
- YAML
- other structured exports if needed later

The emitter layer should be format-specific but conceptually thin. It should not contain core semantic inference logic.

### 7. Optional Aggregation Layer

Some metadata is naturally per-project. Other metadata becomes more useful when aggregated across:

- multiple projects in a solution
- shared libraries and applications
- package boundaries

This suggests an optional aggregation layer later, but it should not be required for the first proof of viability.

## Recommended Build and Data Flow

1. Developers annotate code or rely on supported structural conventions.
2. Roslyn-based analysis discovers symbols, usages, and relationships during compilation.
3. A shared extraction core turns those findings into a normalized semantic fact set.
4. Analyzer diagnostics surface quality issues or unsupported patterns.
5. An MSBuild-integrated emission step converts the extracted facts into external artifacts.
6. The artifacts are placed in intermediate and/or final build outputs for downstream tooling.

This keeps the flow tightly coupled to build activity while avoiding overloading one tool type with every responsibility.

## Boundaries and Responsibilities

### Roslyn-side responsibilities

- inspect syntax trees
- inspect semantic models and symbols
- discover usage patterns
- validate annotation correctness
- produce extraction facts

### Build-side responsibilities

- choose when generation runs
- determine output destinations
- manage enabled artifact types
- package artifacts with build outputs
- coordinate cross-project or publish-time behavior

### Consumer-side responsibilities

Downstream systems may use generated artifacts for:

- infrastructure validation
- permission review
- deployment metadata
- governance or compliance checks

Those consumers should remain outside the core build-analysis engine.

## Suggested Repository Shape

Exact names are not important, but the architecture would benefit from a layout roughly like this:

- `src/Core`
  Shared semantic extraction logic and internal metadata abstractions
- `src/Analyzers`
  Roslyn analyzers and validation rules
- `src/Build`
  MSBuild integration and artifact emission orchestration
- `src/Emitters`
  Output-format-specific artifact writers
- `src/Annotations`
  Optional shared attribute or contract package
- `samples/`
  Small example projects proving the permission and API usage workflow
- `docs/`
  Product notes, format intent, and adoption guidance

The important point is not the folder names. The important point is separating:

- extraction logic
- validation logic
- build orchestration
- output serialization

## Major Technical Risks

### 1. Scope explosion

The current vision names many metadata categories that have different analysis difficulty and different product value.

Risk:

- the system becomes a vague platform instead of a working product

Mitigation:

- define one or two first-class artifact families for v1
- treat all others as extension points, not immediate commitments

### 2. Static analysis blind spots

Some behaviors will not be visible enough for precise inference.

Risk:

- false confidence in generated outputs

Mitigation:

- make conservative modeling explicit
- emit diagnostics when certainty is low
- require annotations where inference is weak

### 3. Over-reliance on source generators

If the design assumes source generators alone will solve discovery, validation, and external file output, the system will likely become awkward.

Risk:

- unclear ownership of output generation
- brittle build behavior
- poor separation of concerns

Mitigation:

- keep semantic discovery near Roslyn
- keep output file lifecycle under MSBuild orchestration

### 4. Build performance and developer experience

Deep semantic analysis can become expensive.

Risk:

- slower builds
- poor IDE responsiveness

Mitigation:

- keep the first rule set narrow
- favor incremental analysis patterns
- measure analysis cost early with realistic sample projects

### 5. Cross-assembly semantics

Many important APIs may live in referenced libraries rather than the current project.

Risk:

- incomplete manifests unless metadata survives across boundaries

Mitigation:

- decide early whether referenced symbols carry enough metadata through attributes alone
- later consider importing generated metadata from dependencies when needed

### 6. Artifact trust and governance

Generated artifacts are only useful if teams trust them.

Risk:

- manifests exist but are ignored because they are perceived as incomplete or unstable

Mitigation:

- make limitations visible
- version outputs carefully
- keep the first outputs simple and inspectable

## Assumptions That Need Validation

- teams are willing to add explicit annotations where pure inference is insufficient
- direct symbol and attribute analysis covers enough real-world value to justify the approach
- downstream consumers actually want build-time generated manifests
- conservative aggregate outputs are acceptable for the first product phase
- per-project artifact generation is useful before solution-level aggregation exists

If these assumptions are false, the architecture may still function technically, but it may not be valuable enough in practice.

## Unresolved Decisions

The following decisions are still open and materially affect the design:

### Initial scope boundary

Should the first version analyze:

- one project at a time
- a full solution
- or published package/application boundaries

This affects both extraction strategy and artifact meaning.

### First-class metadata families

Which outputs matter enough to define the first successful product?

Without this, the platform remains too abstract.

### Source of truth model

Should metadata come primarily from:

- explicit attributes
- structural inference
- or a hybrid of both

This determines reliability, ergonomics, and adoption cost.

### Build behavior on ambiguity

When metadata is incomplete or uncertain, should the system:

- emit best-effort artifacts
- emit warnings
- or fail builds

This is a product decision as much as a technical one.

### Output lifecycle

Should generated artifacts live only in intermediate outputs, or also:

- ship with binaries
- flow into publish outputs
- become package assets

This affects downstream consumption patterns.

### Cross-project aggregation model

Should aggregation happen:

- during each project build
- at solution build time
- or in a later packaging or publish stage

The answer changes both complexity and user expectations.

## Clarifying Questions

These are the questions I would want answered before hardening this into a concrete implementation plan:

1. What is the single most important artifact family for v1: permissions, dependencies, API usage, or something else?
2. Is the first supported analysis boundary a single project build, or does the design need to reason across an entire solution from the beginning?
3. Will important semantic metadata mostly live in the same repository, in referenced internal libraries, or in third-party packages?
4. Are developers expected to add explicit annotations where inference is weak, or is low-annotation inference a core product requirement?
5. Should incomplete metadata fail the build, warn, or simply produce best-effort output?
6. Who is the primary consumer of the generated artifacts: developers, deployment tooling, security review, platform governance, or something else?

## Recommended Next Step

The best next step is not full implementation.

The best next step is a narrowly scoped proof of concept with the following properties:

- one metadata family, preferably permissions
- one supported declaration style, preferably explicit attributes
- one supported discovery path, preferably direct API usage
- one output format, preferably JSON
- one build integration path that proves artifacts can be emitted automatically and predictably

If that proof of concept is successful, the broader platform vision becomes much more credible. If it struggles, the team will learn exactly which assumptions need to change before expanding scope.

## Final Verdict

The overall architecture is coherent and realistically implementable.

The strongest version of the design is a **hybrid Roslyn plus MSBuild system** with:

- explicit semantic declarations in code
- shared extraction logic
- analyzer-driven validation
- build-owned external artifact emission

The biggest blockers are not raw technical impossibility.

They are:

- lack of initial scope discipline
- unclear source-of-truth rules
- unresolved decisions about analysis boundaries, confidence handling, and downstream consumers

If those are resolved, the proposed components fit together well enough to justify a focused proof of concept.
