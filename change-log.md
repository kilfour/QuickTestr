### 0.0.1: The Seedling
* Initial implementation of a QuickCheck-style DSL built on top of QuickCheckr.
* Added Testr versions of the jqwik challenges.
* Added an example showing how complex problems can be handled by *steering* the engine.
* Added an alternative way of defining a Testr tailored for oracle-based testing.

### 0.0.2: Investigating with Double Vision
* Added `Expected` and `Actual` for Redux values in Oracle-based tests.
* Introduced `Testr`-specific `GatheringEvidence`.

### 0.0.3: Fixing the Oracle
* Bugfix release

### 0.0.5: Entering the Vault
* QuickCheckr update.
* Added Xml Summary comments to the public API.
* Renamed the old investigation-style persistence API to the new vault vocabulary, including `FillVault`, `InspectVault`, `CleanupVault`, and related types.