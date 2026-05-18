using QuickTestr.Tests;
using QuickTestr.Tests.Challenges;
using QuickTestr.Tests.Docs;
using QuickTestr.Tests.Examples.DantesCalculator;
using QuickTestr.Tests.Notes;

Explain.This<TestrChallenges>("Docs/challenges.md");
Explain.This<Documentation>("Docs/doc.md");
Explain.This<DantesCalculator>("Examples/dantes-calculator.md");
Explain.This<NotesRoot>("DocsSinglePages/notes.md");

Explain.OnlyThis<CreateReadme>("README.md");