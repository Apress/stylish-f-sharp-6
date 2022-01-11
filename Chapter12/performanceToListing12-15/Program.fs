open System
open BenchmarkDotNet.Running
open BenchmarkDotNet.Attributes

module Harness =
    [<MemoryDiagnoser>]
    type Harness() =
        let r = Random()
        let list = List.init 1_000_000 (fun _ -> r.NextDouble())
        let array = list |> Array.ofList
        [<Benchmark>]
        member _.Old() =
            list
            |> InappropriateCollectionType.Old.sample 1000
            |> ignore
        [<Benchmark>]
        member __.New() =
            list
            |> InappropriateCollectionType.New.sample 1000
            |> Array.ofSeq
            |> ignore

[<EntryPoint>]
let main _ =
    BenchmarkRunner.Run<Harness.Harness>()
    |> printfn "%A"
    0
