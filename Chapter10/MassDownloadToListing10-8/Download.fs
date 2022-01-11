namespace MassDownload

module Download =

    open System
    open System.IO
    open System.Net
    open System.Text.RegularExpressions
    // From Nuget package "FSharp.Data": dotnet add package FSharp.Data
    open FSharp.Data

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
    let private getLinks (pageUri : Uri) (filePattern : string) =

        Log.cyan "Getting names..."
        let re = Regex(filePattern)
        let html = HtmlDocument.Load(pageUri.AbsoluteUri)

        let links =
            html.Descendants ["a"]
            |> Seq.choose (fun node ->
                node.TryGetAttribute("href")
                |> Option.map (fun att -> att.Value()))
            |> Seq.filter (re.IsMatch)
            |> Seq.map (absoluteUri pageUri)
            |> Seq.distinct
            |> Array.ofSeq

        links

    /// Download a file to the specified local path.
    let private tryDownload (localPath : string) (fileUri : Uri) =

        let fileName = fileUri.Segments |> Array.last
        Log.yellow (sprintf "%s - starting download" fileName)

        let filePath = Path.Combine(localPath, fileName)
        use client = new WebClient()

        try
            client.DownloadFile(fileUri, filePath)
            Log.green (sprintf "%s - download complete" fileName)
            Result.Ok fileName
        with
        | e ->
            let message =
                e.InnerException
                |> Option.ofObj
                |> Option.map (fun ie -> ie.Message)
                |> Option.defaultValue e.Message
            Log.red (sprintf "%s - error: %s" fileName message)
            Result.Error e.Message

    /// Download all the files linked to in the specified webpage, whose
    /// link path matches the specified regular expression, to the specified
    /// local path.
    let GetFiles (pageUri : Uri) (filePattern : string) (localPath : string) =

        let links = getLinks pageUri filePattern

        let downloadResults =
            links
            |> Array.map (tryDownload localPath)

        let isOk = function
            | Ok _ -> true
            | Error _ -> false

        let successCount = 
            downloadResults |> Seq.filter isOk |> Seq.length

        let errorCount =
            downloadResults |> Seq.filter (isOk >> not) |> Seq.length
            
        {| 
            SuccessCount = successCount
            ErrorCount = errorCount
        |}

