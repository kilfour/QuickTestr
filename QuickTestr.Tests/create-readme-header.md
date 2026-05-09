> **No fuss. Just Fuzz.**  
> `Named => For => Assert => Run`

[![Docs](https://img.shields.io/badge/docs-QuickTestr-blue?style=flat-square&logo=readthedocs)](https://github.com/kilfour/QuickTestr/blob/main/Docs/doc.md)
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

