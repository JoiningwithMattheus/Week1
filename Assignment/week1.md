# Interaction Design Week 1
Erik Ivar Bokvist Wrammerfors
570354

## Background research

### Common Language Infrastructure (CLI)
The CLI provides an executable code specification as well as providing a unifying infrastructure for creating and executing applications through its components (the CTS, CLS and VES). This unified infrastructure is compatible with several high-level languages and can be used on various computer platforms, essentially allowing those languages to be used without needing to be written with a specific architecture in mind.

refs: 
- ECMA-335_6th_edition_june_2012 I.6
- en.wikipedia.org/wiki/Common_Language_Infrastructure

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

