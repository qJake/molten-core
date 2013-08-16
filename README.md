# Molten Core

Molten Core is a general all-purpose utility framework for .NET 4.0/4.5.

## Features

Molten Core is split up into a few separate assemblies in order to isolate dependencies:

* **Molten.Core** &ndash; General all-purpose utility library
* **Molten.Core.Wpf** &ndash; WPF/MVVM framework utilities
* **Molten.Core.WinApi** &ndash; Win32 utilities and helpers
* **Molten.Core.Tests** &ndash; Unit tests for the entire Molten Core suite

For more information and detailed feature lists, please see the [wiki](https://github.com/qJake/molten-core/wiki).

## Framework Versions

Here is a table detailing each project inside Molten Core and its supported framework versions:

| &nbsp;             | .NET 4.0  | .NET 4.5 | .NET 4.5.1 |
|--------------------|:---------:|:--------:|:----------:|
| Molten.Core        | **Yes**   | **Yes**  | **Yes**    |
| Molten.Core.Wpf    |   No      | **Yes**  | **Yes**    |
| Molten.Core.WinApi | **Yes***  | **Yes**  | **Yes**    |

**&#42;** In order to support the 4.0 framework for the WinApi component, there are two copies of the same method, one which uses `async`/`await` and one that does not. To enable or disable the .NET 4.5 features, set the compilation flag `ENABLE_45_ASYNC` if you are compiling for .NET 4.5. Otherwise, un-set the flag. **The `ENABLE_45_ASYNC` flag is set by default when you clone the repo.**

The default configuration for the solution when you clone the repo is **.NET 4.5 (Visual Studio 2012)**.

## Contributing

To contribute, fork the code, make improvements or add new features that belong in an all-purpose utility library, commit, and send me a pull request.

**Looking for something to contribute?** [Search for `TODO:` in the source](https://github.com/qJake/molten-core/search?q=TODO%3A&ref=cmdform), there are numerous places where improvements can be made!
