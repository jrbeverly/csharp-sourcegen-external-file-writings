# Root Makefile for the sourcegen external-file-writing solution.

SLN := PermissionManifest.slnx
MANIFEST := src/Sample/bin/Debug/net9.0/permissions.json

.DEFAULT_GOAL := help
.PHONY: help build manifest clean

help: ## Show available targets
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) \
		| awk 'BEGIN {FS = ":.*?## "} {printf "  \033[36m%-12s\033[0m %s\n", $$1, $$2}'

build: ## Build the solution (runs the generator and emits permissions.json)
	dotnet build $(SLN)

manifest: ## Print the emitted permissions.json
	@cat $(MANIFEST)
	@echo

clean: ## Remove build outputs (bin/obj) across the solution
	dotnet clean $(SLN)
