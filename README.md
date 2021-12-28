# Hilite

Hilite is a console-based syntax highlighter for a number of language files
and scripts, including (but not limited to) batch files,JavaScript, VBScript,
Visual Basic, C#, SQL, Python, HTML, XML, XHTML and XSLT. It makes quick
reading of source code files a lot easier without leaving the Windows Command
Prompt. Below, you can see a batch file displayed using the regular `type`
command and the `hilite` utility (note syntax highlighting on comments like
`rem`, known commands like `echo` and `cd` and even environment variables):

![Hilite Screenshot](http://raboof.com/projects/hilite/Hilite.jpg)

Hilite employs an algorithm that was borrowed from the JavaScript toolset
[dp.SyntaxHighlighter] by [Alex Gorbatchev]. It is not perfect and sometimes
you might see a few odd cases of highlighting, but it's fairly effective for
90% of the cases.

[dp.SyntaxHighlighter]: http://code.google.com/p/syntaxhighlighter/
[Alex Gorbatchev]: http://www.dreamprojections.com/

## Usage

    Hilite [ ( FILENAME | - | ? ) [ LANGUAGE ] ]

The first argument can either be a file name or the dash (-) character. If it
is dash then standard input is used. In general, the tool can guess the syntax
highlighting to apply from the file extension. For example, JavaScript for
`.js`, SQL for `.sql`, HTML for `.htm` and so on. In cases where the extension is
not obvious or cannot be inferred, you can specify the language as the second
argument. This is needed, for instance, if you are reading from standard
input. To see a list of supported language names, aliases and extensions, run
`hilite ?`; that is, using question mark (?) as the first and only argument.
