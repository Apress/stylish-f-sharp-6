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
        [|
            let max =
                ( (data |> Array.length |> float) / (float interval)
                  |> ceil
                  |> int ) - 1
            for i in 0..max ->
                 data.[i * interval]
        |]




