open System
open System.Diagnostics
open MassDownload

[<EntryPoint>]
let main args =

    // A program to get multiple files from download links provided on a website.
    // (Quite naive - intended mainly as a basis for demonstrating async programming.)

    // E.g. dotnet run https://minorplanetcenter.net/data "neam.*\.json\.gz$" "c:\temp\downloads"
    //      dotnet run http://compling.hss.ntu.edu.sg/omw "\.zip$" "c:\temp\downloads"
    //
    //   Large!
    //      dotnet run http://storage.googleapis.com/books/ngrams/books/datasetsv2.html "eng\-1M\-2gram.*\.zip$" "c:\temp\downloads"

    if args.Length = 3 then
        let uri = Uri args.[0]
        let pattern = args.[1]
        let localPath =args.[2]
        let sw = Stopwatch()
        sw.Start()

        let result =
            Download.GetFiles uri pattern localPath
            
        Log.cyan
            (sprintf "%i files downloaded in %0.1fs, %i failed."
                result.SuccessCount sw.Elapsed.TotalSeconds result.ErrorCount)
        0
    else
        Log.red @"Usage: massdownload url nameregex download path - e.g. massdownload https://minorplanetcenter.net/data neam.*\.json\.gz$ c:\temp\downloads"
        1
