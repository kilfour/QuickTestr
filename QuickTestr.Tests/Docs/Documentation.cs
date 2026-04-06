using QuickPulse.Explains;
using QuickTestr.Tests.Docs.A_PropertyBased;
using QuickTestr.Tests.Docs.B_OracleBased;

namespace QuickTestr.Tests.Docs;

[DocFile]
[DocFileHeader("QuickTestr")]
[DocLink(typeof(PropertyBased))]
[DocLink(typeof(OracleBased))]
[DocContent("""
QuickTestr currently supports two styles:
- [Property-based](PropertyBased) : Define what should always hold.
- [Oracle-based](OracleBased): Compare against something that already works.
""")]
public class Documentation { }