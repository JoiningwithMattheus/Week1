# Interaction Design Week 1

Erik Ivar Bokvist Wrammerfors
570354

## Background research

### Common Language Infrastructure (CLI)

The CLI provides an executable code specification as well as providing a unifying infrastructure for creating and executing applications through its components (the CTS, CLS and VES). This unified infrastructure is compatible with several high-level languages and can be used on various computer platforms, essentially allowing those languages to be used without needing to be written with a specific architecture in mind.
-# refs: 
- ECMA-335_6th_edition_june_2012 I.6
- 

### Virtual Execution System (VES)

The VES provides the support needed to execute instructions of the CIL. It supports a set of built-in data types. It also defines a hypothetical machine consisting of a model and state, control flow constructs, and exception handling.

refs: 
- ECMA-335_6th_edition_june_2012 I.12

### Common Intermediate Language (CIL)
The CIL is the language that the source code of CLI compliant languages compiles to rather than platform- or processor-specific code. It cane be executed in any CLI supporting environment. In CIL, methods are stored as instruction blocks, whose address and length is specified in the file format. each instruction in the block is stored in an exact number of bytes, and the next instruction begins on the next byte. It is impossible to test with complete accuracy if CLI/CIL code is memory safe, so there is instead a stricter restriction called verfiability. All verified code is memory safe, but there can be non-verified code that is also memory safe.

refs:
- ECMA-335_6th_edition_june_2012 III.1.7.1, III.1.8
- en.wikipedia.org/wiki/Common_Intermediate_Language

### Common Type System (CTS)
The CTS is a model type system that defines the rules the CLI follows when interacting with types. It establishes cross-language integration enabled framework shared by the CLI, tools, and compilers. The CTS supports object-oriented programming, and as such it deals with both values and objects. Values are simple types composed of bit patterns for things like integers and floats. Objects are store their type in their own representation and can store other entities, which can be values or more objects. The CTS also has a feature called generics that allows a pattern to be used to define a whole family of types and methods, including having generic parameters than can be replaced with specific types when needed. Generics are designed to be language and implementation independant. 

refs:
- ECMA-335_6th_edition_june_2012 I.5, I.8


### Common Language Specification (CLS)

CLS is a part of CTS and usage conventions.
Language designers access framworks (classes, interfaces, methods, and so on) via parts of CTS. These are included in CLS. It's a connection with conventions.
-#ref: ECMA-335_6th_edition_june_2012.pdf I.6

### Class libraries (.NET SDK)

They are shared library helping users put all the functionality into modules that are used by multiple applications.
There are three types of class libraries:
-Platform-speicific: access to all APIs (e.g. .NET Frameworks on Windows)
-Portable: access only to a subset of APIs (e.g. .NET Framework 4.5+, Windows Phone 8.0)
-NET standard: The combination of both (e.g. .NET Core)
-#ref: https://learn.microsoft.com/en-us/dotnet/standard/class-libraries

### Just-in-time compiling (JIT)

Just-in-time (JIT) compilation is a technique that compiles code into machine code during program execution, rather than before it. This dynamic compilation process aims to improve performance by translating code, such as bytecode, into native machine code at runtime, often focusing on frequently used sections of the program.
-#ref: [Just in time Freecodecamp](https://www.freecodecamp.org/news/just-in-time-compilation-explained/#:~:text=Just%2Din%2Dtime%20compilation%20is,paths%20that%20are%20frequently%20used.)

### Framework (.NET Framework)

The ".NET framework" is a software development platform by Microsoft that allows developers to build and run applications, primarily for Windows. It consists of a set of libraries and a common language runtime (CLR) for executing code written in languages like C# or Visual Basic.
-#ref: https://dotnet.microsoft.com/en-us/learn/dotnet/what-is-dotnet-framework#:~:text=NET%20Framework%20is%20a%20software,types%2C%20and%20deliver%20high%20performance.

## Difference between .NET Core & .NET Framework

| feature            | .NET Core                       | .NET Framework                      | .NET (5/6/7)                  |
| ------------------ | ------------------------------- | ----------------------------------- | ----------------------------- |
| Platform           | Cross platform                  | Windows only                        | Cross platform                |
| Open source        | Yes                             | No                                  | Yes                           |
| Performance        | High                            | Moderate                            | High                          |
| Deployment         | Self-contained or system-wide   | Installed system-wide               | Self-contained or system-wide |
| Latest Development | .NET 5+                         | Legacy, no new features Replaced by | Actively developed            |
| Use case           | Modern web, cloud, console apps | Old enterprise apps                 | Everything modern             |

-#ref: https://learn.microsoft.com/en-us/dotnet/core/porting/ https://learn.microsoft.com/en-us/dotnet/core/introduction