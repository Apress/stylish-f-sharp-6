module InappropriateCollectionType

module Old =
    let sample interval data =
        [
            let max = (List.length data) - 1
            for i in 0..interval..max ->
                data.[i]
        ]
        
module New =
    let sample interval data =
        data
        |> Seq.indexed
        |> Seq.filter (fun (i, _) ->
            i % interval = 0)
        |> Seq.map snd


