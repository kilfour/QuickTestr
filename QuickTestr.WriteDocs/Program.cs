using QuickPulse.Explains;
using QuickTestr.Tests.Docs;
using QuickTestr.Tests.Notes;

Explain.These<Documentation>("Docs/");
Explain.These<NotesRoot>("Notes/");


Explain.This<Documentation>("DocsSinglePages/doc.md");
Explain.This<NotesRoot>("DocsSinglePages/notes.md");