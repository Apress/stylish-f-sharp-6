open System
open BenchmarkDotNet.Running
open BenchmarkDotNet.Attributes

module Harness =
    [<MemoryDiagnoser>]
    type Harness() =
        let r = Random(1)
        let coords =
            Array.init 1_000_000 (fun _ ->
                r.NextDouble(), r.NextDouble(), r.NextDouble())
        let here = (0., 0., 0.)
        let coordsStruct =
            coords
            |> Array.map (fun (x, y, z) -> struct(x, y, z))
        let hereStruct = struct(0., 0., 0.)
        [<Benchmark>]
        member __.Old() =
            coords
            |> ShortTermObjects.Old.withinRadius 0.1 here
            |> ignore
        [<Benchmark>]
        member __.New() =
            coordsStruct
            |> ShortTermObjects.New.withinRadius 0.1 hereStruct
            |> ignore

[<EntryPoint>]
let main _ =
    BenchmarkRunner.Run<Harness.Harness>()
    |> printfn "%A"
    0
