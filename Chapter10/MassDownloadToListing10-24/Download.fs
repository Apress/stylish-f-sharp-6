namespace MassDownload

module Download =

    open System
    open System.IO
    open System.Net
    open System.Text.RegularExpressions
    // From Nuget package "FSharp.Data": dotnet add package FSharp.Data
    open FSharp.Data
    open System.Linq
    // // From Nuget package "TaskBuilder.fs": dotnet add package TaskBuilder.fs
    // TODO Remove when F# 6 is released
    //open FSharp.Control.Tasks.V2

    /// If a download link starts with http: or https: return a Uri of it unchanged,
    /// otherwise return a uri of it relative to its page.
    let private absoluteUri (pageUri : Uri) (filePath : string) =
        if filePath.StartsWith("http:")
           || filePath.StartsWith("https:") then
            Uri(filePath)
        else
            let sep = '/'
            filePath.TrimStart(sep)
            |> (sprintf "%O%c%s" pageUri sep)
            |> Uri
            
    /// Get the URLs of all links in a specified page matching a
    /// specified regex pattern.
    let private getLinksAsync (pageUri : Uri) (filePattern : string) =
        task {
            Log.cyan "Getting names..."
            let re = Regex(filePattern)
            let! html = HtmlDocument.AsyncLoad(pageUri.AbsoluteUri)

            let links =
                html.Descendants ["a"]
                |> Seq.choose (fun node ->
                    node.TryGetAttribute("href")
                    |> Option.map (fun att -> att.Value()))
                |> Seq.filter (re.IsMatch)
                |> Seq.map (absoluteUri pageUri)
                |> Seq.distinct
                |> Array.ofSeq

            return links
        }

    /// Download a file to the specified local path.
    let private tryDownloadAsync (localPath : string) (fileUri : Uri) =
        task {
            let fileName = fileUri.Segments |> Array.last
            Log.yellow (sprintf "%s - starting download" fileName)

            let filePath = Path.Combine(localPath, fileName)
            use client = new WebClient()

            try
                do!
                    client.DownloadFileTaskAsync(fileUri, filePath)
                Log.green (sprintf "%s - download complete" fileName)
                return (Result.Ok fileName)
            with
            | e ->
                let message =
                    e.InnerException
                    |> Option.ofObj
                    |> Option.map (fun ie -> ie.Message)
                    |> Option.defaultValue e.Message
                Log.red (sprintf "%s - error: %s" fileName message)
                return (Result.Error e.Message)
        }

    /// Download all the files linked to in the specified webpage, whose
    /// link path matches the specified regular expression, to the specified
    /// local path.
    let GetFilesAsync (pageUri : Uri) (filePattern : string) (localPath : string) =

        // This could equally well be a parameter:
        let throttle = 5

        task {
            let isOk = function
                | Ok _ -> true
                | Error _ -> false

            let! links = getLinksAsync pageUri filePattern

            let! downloadResults =
                links
                    .AsParallel()
                    .WithDegreeOfParallelism(throttle)
                    .Select(fun uri -> tryDownloadAsync localPath uri)
                |> System.Threading.Tasks.Task.WhenAll

            let successCount = 
                downloadResults |> Seq.filter isOk |> Seq.length

            let errorCount =
                downloadResults |> Seq.filter (isOk >> not) |> Seq.length

            return  
                {| 
                    SuccessCount = successCount
                    ErrorCount = errorCount
                |}
        }

