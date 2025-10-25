# FlaschenCalculator

Smoll calculator application built with C# and Blazor WebAssembly that uses Abstract Syntax Tree (AST) pattern for mathematical expression evaluation.
**Try it out yourself here: https://neutralenull.github.io/FlaschenCalculator/**

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Architecture](#architecture)
- [How It Works](#how-it-works)
  - [Abstract Syntax Tree (AST)](#abstract-syntax-tree-ast)
  - [Parser Implementation](#parser-implementation)
  - [Calculation Process](#calculation-process)
- [Project Structure](#project-structure)
- [How to Run](#how-to-run)
- [Technical Details](#technical-details)

## Overview

FlaschenCalculator is a show off web-based calculator (or console if you prefer lol) that goes beyond simple arithmetic operations. It uses a recursive descent parser to convert mathematical expressions into an Abstract Syntax Tree, which is then evaluated to produce accurate results. The application supports decimal precision up to 7 digits after the decimal point and handles complex expressions with proper operator precedence.
I've got the idea to use AST because of my recent endeavors doing Javascript Code parsing in University. 

## Features

### Core Capabilities (Task Requirements)
- ✅ **Two Number Input with Operations**: Users can enter multiple numbers and perform calculations
- ✅ **Basic Operations**: Addition (+), Subtraction (-), Multiplication (×), Division (÷)
- ✅ **Decimal Support**: As required we got ludicrously precision up to at least 7 digits after the dot using the dotnet decimal datatype. 
- ✅ **User-Friendly Interface**: Super ultra modern, amazingly intuitive button-based calculator with visual feedback 

### Advanced Features (Beyond Requirements)
- 🚀 **Complex Expressions**: Support for multi-operand expressions like `1 + 2 - 3 * 4 / 5 + 6`
- 🔢 **Parentheses**: Grouping support with nested parentheses `(3 + 4) * (5 - 2)`
- 📐 **Additional Operations**:
  - Exponentiation (^): `2^3 = 8`
  - Square Root (√): `sqrt(16) = 4`
- 🧮 **Implicit Multiplication**: `3(2+2) = 12`, `2sqrt(4) = 4`
- 🌳 **Expression Tree Visualization**: View the internal AST representation of expressions
- ⌫ **Smart Backspace**: Removes entire keywords (like "sqrt") in one action
- 🛡️ **Error Prevention**:
  - Division by zero detection
  - Invalid expression syntax checking
  - Unbalanced parentheses detection
  - Invalid character handling

## Architecture

The application follows **Clean Architecture™** principles with clear separation of concerns:

```
FlaschenCalculator
├── Core Layer (Domain)
│   ├── Interfaces: IExpression
│   ├── Models: Expression types (NumberExpression, BinaryExpression, UnaryExpression)
│   ├── Enums: Operation types
│   └── Exceptions: Domain-specific exceptions
├── Infrastructure Layer (Implementation)
│   └── NumberParser: Converts text to AST
├── Presentation Layer (UI)
│   ├── Blazor WebAssembly: Modern web interface
│   └── Console: Command-line interface
└── Test Projects
    ├── Core.Test: Domain logic tests
    └── Infrastructure.Test: Parser tests
```

### Benefits of This Architecture:
- **Independence**: Core logic is independent of UI and frameworks
- **Testability**: Each layer can be tested in isolation
- **Maintainability**: Changes in one layer don't affect others
- **Flexibility**: Multiple UIs (Blazor, Console) share the same core

## How It Works

### Abstract Syntax Tree (AST)

The AST is a tree representation of the mathematical expression where:
- **Leaf nodes** represent numbers (NumberExpression)
- **Branch nodes** represent operations (BinaryExpression, UnaryExpression)

#### Example: Expression `3 + 4 * 2`

```
Tree Structure:
       +
      / \
     3   *
        / \
       4   2
```

The AST automatically respects operator precedence because multiplication nodes are placed lower in the tree, ensuring they are evaluated first.

#### AST Node Types

**1. IExpression (Interface)**
```csharp
public interface IExpression
{
    decimal Evaluate();              // Calculates the result
    string ToString(...);            // Visualizes the tree
    string GetExpressionType();      // Returns node type
}
```

**2. NumberExpression (Leaf Node)**
- Stores a decimal value
- `Evaluate()` simply returns the stored value
- Example: `NumberExpression(5)` → evaluates to `5`

**3. BinaryExpression (Branch Node)**
- Stores an operation (+, -, *, /, ^)
- Has left and right child expressions
- `Evaluate()` recursively evaluates both children, then applies the operation
- Example: `BinaryExpression(+, NumberExpression(3), NumberExpression(4))` → evaluates to `7`

**4. UnaryExpression (Single Branch Node)**
- Stores an operation (+, -, sqrt)
- Has one child expression
- `Evaluate()` evaluates the child and applies the unary operation
- Example: `UnaryExpression(-, NumberExpression(5))` → evaluates to `-5`

### Parser Implementation

The `NumberParser` uses a **Recursive Descent Parser** algorithm, which:
1. Reads the input character by character
2. Builds the AST according to a formal grammar
3. Respects operator precedence through the parsing order

#### Grammar Rules (BNF-like notation)

```
expression       → addSubExpression
addSubExpression → mulDivExpression ( ("+" | "-") mulDivExpression )*
mulDivExpression → exponentiation ( ("*" | "/" | implicit) exponentiation )*
exponentiation   → factor ("^" exponentiation)?
factor           → NUMBER 
                 | "(" expression ")" 
                 | ("+" | "-" | "sqrt") factor
```

#### Parsing Steps for `3 + 4 * 2`:

1. **Start**: Parse expression → Parse addition/subtraction level
2. **Step 1**: Parse `3` → Create `NumberExpression(3)`, store as left operand
3. **Step 2**: See `+` operator → Continue parsing
4. **Step 3**: Parse `4 * 2` at higher precedence (multiplication level)
   - Parse `4` → Create `NumberExpression(4)`, store as left
   - See `*` → Continue
   - Parse `2` → Create `NumberExpression(2)`, store as right
   - Create `BinaryExpression(*, 4, 2)` and return it
5. **Step 4**: Create `BinaryExpression(+, 3, BinaryExpression(*, 4, 2))`
6. **Result**: AST with correct precedence

#### Key Parser Features:

- **Operator Precedence**: Handled by parsing order (addition → multiplication → exponentiation → factors)
- **Right Associativity**: Exponentiation is right-associative (`2^3^2 = 2^(3^2) = 512`)
- **Parentheses**: Force higher-precedence expressions to be parsed as factors
- **Implicit Multiplication**: `3(4)` automatically inserts multiplication
- **Error Detection**: Validates syntax and throws `FormatException` for invalid input

### Calculation Process

The complete flow from input to result:

```
User Input: "3 + 4 * 2"
     ↓
[1. Tokenization & Parsing]
  NumberParser.Parse("3 + 4 * 2")
     ↓
[2. AST Construction]
       +
      / \
     3   *
        / \
       4   2
     ↓
[3. Evaluation (Post-Order Traversal)]
  - Evaluate left child: 3 → 3
  - Evaluate right child:
    - Evaluate left child: 4 → 4
    - Evaluate right child: 2 → 2
    - Apply operation: 4 * 2 → 8
  - Apply operation: 3 + 8 → 11
     ↓
[4. Result Display]
  Result: 11
```

#### Evaluation Algorithm (Recursive):

```csharp
// Pseudo-code for BinaryExpression.Evaluate()
decimal Evaluate()
{
    decimal leftValue = left.Evaluate();    // Recursively evaluate left
    decimal rightValue = right.Evaluate();  // Recursively evaluate right
    
    return operation switch
    {
        Addition       => leftValue + rightValue,
        Subtraction    => leftValue - rightValue,
        Multiplication => leftValue * rightValue,
        Division       => leftValue / rightValue,  // with zero check
        Power          => Math.Pow(leftValue, rightValue)
    };
}
```

## Project Structure

```
FlaschenPost/
│
├── FlaschenCalculator.Core/                    # Domain Layer
│   ├── Interfaces/
│   │   └── IExpression.cs                      # Base interface for all AST nodes
│   ├── Models/
│   │   ├── NumberExpression.cs                 # Leaf nodes (numbers)
│   │   ├── BinaryExpression.cs                 # Binary operations (+, -, *, /, ^)
│   │   ├── UnaryExpression.cs                  # Unary operations (-, +, sqrt)
│   │   └── Operation.cs                        # Operation enum
│   └── Exceptions/
│       ├── InvalidOperationException.cs        # Invalid operation errors
│       └── InvalidOperationDivideByZeroException.cs
│
├── FlaschenCalculator.Infrastructure/          # Implementation Layer
│   └── NumberParser.cs                         # Recursive descent parser
│
├── FlaschenCalculator.Presentation.Blazor/     # Web UI
│   ├── Calculator.razor                        # Main calculator component
│   ├── Program.cs                              # Entry point
│   └── wwwroot/                                # Static assets
│
├── FlaschenCalculator.Presentation.Console/    # Console UI
│   └── Program.cs                              # REPL interface
│
├── FlaschenCalculator.Core.Test/               # Domain tests
└── FlaschenCalculator.Infrastructure.Test/     # Parser tests
    └── NumberParserTests.cs                    # Comprehensive test suite
```

## How to Run

### Prerequisites
- .NET 9.0 SDK or later
- A modern web browser (for Blazor version)

### Running the Blazor Web Application

```bash
cd FlaschenCalculator.Presentation.Blazor
dotnet run
```

Then open your browser to the URL shown (typically `http://localhost:5000`)

### Running the Console Application

```bash
cd FlaschenCalculator.Presentation.Console
dotnet run
```

Type mathematical expressions and press Enter. Type `exit` to quit.

### Running Tests

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test FlaschenCalculator.Infrastructure.Test
```

## Technical Details

### Technologies Used
- **Language**: C# 12 (.NET 9.0)
- **Frontend**: Blazor WebAssembly
- **Architecture Pattern**: Clean Architecture
- **Design Patterns**: 
  - Composite Pattern (AST nodes)
  - Visitor Pattern (implicit in Evaluate())
  - Strategy Pattern (operation types)
- **Parsing Technique**: Recursive Descent Parser
- **Testing**: NUnit framework

### Performance Optimizations
- **ref struct**: Parser uses `ref struct` to avoid heap allocations
- **ReadOnlySpan<char>**: Zero-copy string parsing
- **decimal Type**: Optimized for financial calculations with no floating-point errors

### Supported Expression Examples

```
Basic Operations:
  5 + 3                     →  8
  10 - 4                    →  6
  7 * 6                     →  42
  15 / 3                    →  5

Decimal Numbers:
  3.14159 + 2.71828         →  5.85987
  0.1 + 0.2                 →  0.3 (exact, no floating-point error!)

Complex Expressions:
  2 + 3 * 4                 →  14 (respects precedence)
  (2 + 3) * 4               →  20 (parentheses override)
  6 / 2 * (1 + 2)           →  9
  1 + 2 - 3 * 4 / 5 + 6     →  6.6

Advanced Features:
  2^3                       →  8
  2^3^2                     →  512 (right-associative)
  sqrt(16)                  →  4
  3(2+2)                    →  12 (implicit multiplication)
  -5 + 3                    →  -2 (unary minus)

Error Cases:
  5 / 0                     →  Error: Division by zero
  (2 + 3                    →  Error: Missing closing parenthesis
  2 + @                     →  Error: Invalid character
```
