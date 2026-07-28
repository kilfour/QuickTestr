using QuickPulse.Explains;
using QuickTestr.Tests.Docs.A_PropertyBased;
using QuickTestr.Tests.Docs.B_OracleBased;
using QuickTestr.Tests.Docs.C_ModelBasedTesting;
using QuickTestr.Tests.Docs.D_ExplorationBasedTesting;

namespace QuickTestr.Tests.Docs;

[DocFile]
[DocFileHeader("QuickTestr")]
[DocLink(typeof(PropertyBased))]
[DocLink(typeof(OracleBased))]
[DocLink(typeof(ModelBased))]
[DocLink(typeof(EplorationBased))]
[DocContent("""
QuickTestr currently supports four styles:
- [Property-based][PropertyBased] : Define what should always hold.
- [Oracle-based][OracleBased]: Compare against something that already works.
- [Model-based][ModelBased]: Compare state transitions against a model.
- [Eploration-based][EplorationBased]: TODO.
""")]
public class Documentation { }