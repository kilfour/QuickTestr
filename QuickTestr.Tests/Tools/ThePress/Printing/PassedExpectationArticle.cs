using QuickCheckr.UnderTheHood.Proceedings.Depositions;

namespace QuickTestr.Tests.Tools.ThePress.Printing;

public class PassedExpectationArticle(PassedExpectationDeposition deposition)
    : AbstractArticle<PassedExpectationDeposition>(deposition);
