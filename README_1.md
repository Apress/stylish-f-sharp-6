# Stylish F# 6

This is the source code for the book "Stylish F# 6" by Kit Eason, Apress 2021.

## Contents

Although the book is targeted at F# 6, F# is a slowly-evolving language and you
should find that almost all of the code works in F# 5 - or even earlier versions.

Most of the code samples are provided in the form of .NET Interactive notebooks.  The easiest
way to access them is to install Visual Studio Code and add the ".NET Interactive Notebooks"
extension.

You can then run each of the samples by clicking the 'Run' button next to the cell.

In a few cases the code deliberately raises an exception. Where this is the case there is a
comment near the end of the listing, indicating which exception should be raised. In a very
few cases the code (deliberately) does not compile. Again this is called out in comments
and in the text of the book.

Chapters 10 and 12 require a more project-based approach. For these there is a folder
for the chapter. Within that is a subfolder for each state of the project, as we build it up
in the successive listings in the chapter itself. For example the folder

 `Chapter10/MassDownloadToListing10-8`

...has the state of the project up to and including Listing 10-8.

There are no listings for Chapter 1, as this chapter only contains listings for hypothetical
examples of bad code, which you won't want to attempt to run!

At the time of writing, .NET Interactive Notebooks render `None` values as `<null>`. You 
should bear this in mind when interpreting outputs from code run in Notebooks.

## Exercises

Each notebook ends with an "Exercises" and an "Excercise Solutions" section. The "Exercises"
section, or individual listings, may be omitted when there is no starting code for the exercise.

## Disclaimer

*This is sample code only. You should not use it, modified or unmodified, unless you have
a full understanding of the consequences.*

## Example Data Files

One data file is provided: MPCORB.DAT, courtesy of the Minor Planet Center. The file is in 
compressed form and you will need to 'unzip' it before running some of the samples for
Chapter 13.  Alternatively you can download the latest version from:

https://www.minorplanetcenter.net/iau/MPCORB/MPCORB.DAT

## Array Element Syntax

For backwards compatibility, these code samples use a dot when accessing array elements, e.g.

    let x = a.[42]

From F# 6.0, the '.' can be omitted, e.g.

    let x = a[42]

I recommend omitting the '.' for code that is only going to be compiled using the F# 6.0
or later compiler.
