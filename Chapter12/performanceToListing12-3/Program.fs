open System
open BenchmarkDotNet.Running
open BenchmarkDotNet.Attributes

module Harness =
    [<MemoryDiagnoser>]
    type Harness() =
        [<Benchmark>]
        member _.Old() =
            Dummy.slowFunction()
        [<Benchmark>]
        member _.New() =
            Dummy.fastFunction()

[<EntryPoint>]
let main _ =
    BenchmarkRunner.Run<Harness.Harness>()
    |> printfn "%A"
    0
