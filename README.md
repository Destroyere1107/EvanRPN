# EvanRPN

A relatively simple and Reverse Polish Notation (RPN) engine implemented in C#.

## Features

- **Basic Arithmetic**: Addition, Subtraction, Multiplication, Division, Modulo.
- **Advanced Math**: Power (`^`), Roots (`xroot`), Square Root (`sqrt`), Absolute Value (`abs`), Reciprocal (`1/x`).
- **Stack Manipulation**: Swap, Duplicate (`dup`), Drop, Clear.
- **Constants**: Pi ($\pi$), Euler's number ($e$), and Avogadro's Number.
- **Flexible Interface**: Support for both direct method calls and string-based command execution (for console
  apps/testing).

### Prerequisites

- .NET 10.0 SDK or later. (This will probably work on some older versions, but it was developed for .NET 10.)

### Usage

Spawn the `Engine` class to make a stack and perform calculations:

```csharp
using EvanRPN;

var engine = new Engine();

// Calculate (3 + 4) * 5
engine.Push(3);
engine.Push(4);
engine.Add();
engine.Push(5);
engine.Multiply();

Console.WriteLine($"Result: {engine.Peek()}"); // Output: 35
```

Alternatively, use the `Execute` method for command-based operations:

```csharp
engine.Push(10);
engine.Push(2);
engine.Execute("/"); // Division
engine.Execute("sqrt"); // Square root of the result
```

## Supported Commands exposed by Engine.Execute

These are passed to the engine as text. See testconsole.cs for an example of how to use these.

| Command        | Operation      | Description                       |
|:---------------|:---------------|:----------------------------------|
| `+`, `add`     | Addition       | Adds top two values               |
| `-`, `sub`     | Subtraction    | Subtracts top from second         |
| `*`, `mul`     | Multiplication | Multiplies top two values         |
| `/`, `div`     | Division       | Divides second by top             |
| `%`, `mod`     | Modulo         | Remainder of second / top         |
| `^`, `pow`     | Power          | Second raised to the power of top |
| `neg`, `chs`   | Negate         | Multiplies top by -1              |
| `abs`          | Absolute       | Absolute value of top             |
| `sqrt`         | Square Root    | Square root of top                |
| `xroot`        | X-th Root      | Top-th root of second             |
| `1/x`, `inv`   | Reciprocal     | 1 / top                           |
| `swap`         | Swap           | Swaps top two values              |
| `dup`          | Duplicate      | Duplicates top value              |
| `drop`         | Drop           | Removes top value                 |
| `clear`, `clr` | Clear          | Clears the stack                  |
| `pi`           | Constant       | Pushes $\pi$                      |
| `e`            | Constant       | Pushes $e$ (Euler's number)       |

## Running

There are three csproj files included:

| Project                    | Type          | Purpose                                                                         |
|----------------------------|---------------|---------------------------------------------------------------------------------|
| `EvanRPN`       | Library       | The `Engine` class (the whole library as of now).                               |
| `EvanRPN.Tests` | xUnit tests   | Runs a variety of tests against the Engine.                                     |
| `manualtesting`            | Console (Exe) | A simple REPL app that runs in the console, for manually testing/experimenting. |
