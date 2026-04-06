# <img src='icon.png' width='40' align='top'/> QuickTestr
> **No fuss. Just Fuzz.**  
> `Named => For => Assert => Run`

[![Docs](https://img.shields.io/badge/docs-QuickTestr-blue?style=flat-square&logo=readthedocs)](https://github.com/kilfour/QuickTestr/blob/main/Docs/ToC.md)
[![NuGet](https://img.shields.io/nuget/v/QuickTestr.svg?style=flat-square&logo=nuget)](https://www.nuget.org/packages/QuickTestr)
[![License: MIT](https://img.shields.io/badge/license-MIT-success?style=flat-square)](https://github.com/kilfour/QuickTestr/blob/main/LICENSE)


**QuickTestr** is a small, opinionated DSL built on top of **QuickCheckr**.
It is meant for the cases where you want the power of property-based testing, but do not need the full stateful workflow.

Where **QuickCheckr** is designed for sequences of actions, evolving state, pools, and behavioural shrinking,
**QuickTestr** focuses on the more traditional shape of a property:

* Generate input.
* Assert an invariant.
* Get a useful counterexample when it fails.

It is still powered by the QuickCheckr engine underneath, which means you keep the same emphasis on explainable failures,
transparent reporting, and domain-guided shrinking.

If your test is basically "for all generated values, this should hold", **QuickTestr** is probably the nicer entry point.
If your bug only shows up after a sequence of operations on the same object, reach for **QuickCheckr**.

You don't really need to know about **QuickCheckr** when using this library, but understanding input generation is useful in practice.  
**QuickCheckr** uses [**QuickFuzzr**](https://github.com/kilfour/QuickFuzzr/blob/main/README.md) for its random input generation.

## Example

Here is a deliberately small example:

```csharp
Testr
    .Named("Reversing a list of integers results in the same list")
    .For(Fuzzr.Int().Many(0, 10).ToList())
    .Assert(a =>
    {
        var reversed = new List<int>(a);
        reversed.Reverse();
        return reversed.SequenceEqual(a);
    });
```

That property is false, of course, and QuickTestr reports a shrunk counterexample:

```text
------------------------------------------------------------
  Reversing a list of integers results in the same list
  Seed: 12901993
 ------------------------------------------------------------
  Falsified:
    Input = [ _, 94 ]
    Redux = [ _, 0 ]

  Original:
    [ 76, 92, 75, 6, 94 ]
 ------------------------------------------------------------
```

QuickTestr keeps the surface area small, while still giving you:

* The original failing value.
* A reduced version when reduction is enabled.
* A reproducible seed.
* A report format that is compact enough for day-to-day use.

## Why QuickTestr?

There are already strong property-based testing tools in .NET.
QuickTestr is not trying to replace all of them.

Its niche is:

* **C#-friendly fluency.**
* **Small API surface.**
* **Simple reports.**
* **Easy opt-in custom shrinking.**
* **Integration with the QuickCheckr worldview.**

In other words: less framework energy, more getting on with the test.

## Highlights

* **Tiny DSL:** `Named`, `For`, `Assert`, `Run`, and a small set of focused extras.
* **Built on QuickCheckr:** Benefits from the same shrinking engine and reporting philosophy.
* **Custom reducers:** Plug in domain-aware shrinking when built-ins are not enough.
* **Deterministic:** Failures come with seeds, so rerunning is straightforward.
* **Good for oracle tests:** Compare a buggy implementation against a trusted model with minimal ceremony.

## Basic Usage

### Define a property

```csharp
Testr.Named("The maximum value of the list is smaller than 900.")
    .For(
        from length in Fuzzr.Int(1, 100)
        from list in Fuzzr.Int(0, 1000).Many(length)
        select list.ToList())
    .Assert(a => a.Max() < 900);
```

### Run it

```csharp
Testr.Named("example")
    .For(Fuzzr.Int())
    .Assert(x => x != 42)
    .Run();
```

By default, `Run()` performs the default number of runs.
You can also supply explicit run counts or a seed, depending on how you want to work.

## Installation

QuickTestr is available on NuGet:

```bash
Install-Package QuickTestr
```

Or via the .NET CLI:

```bash
dotnet add package QuickTestr
```

## Current Direction

QuickTestr is intentionally small, but it is growing in useful directions.
Current and near-future areas include:

* Better defaults around deliberation.
* More built-in reducers.
* Continued work on report clarity.
* More examples drawn from real shrinking challenges.

## License

This project is licensed under the [MIT License](https://github.com/kilfour/QuickTestr/blob/main/LICENSE).

