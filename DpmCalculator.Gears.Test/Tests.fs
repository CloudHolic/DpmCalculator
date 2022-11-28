namespace DpmCalculator.Items.Test

module Tests =
    open System.IO
    open Xunit
    open FSharp.Json
    open DpmCalculator.Items.Gear
    open DpmCalculator.Items.SetEffect

    [<Fact>]
    let ParseItemDataTest () =
        let data =
            @"C:\Projects\DpmCalculator\DpmCalculator.Items\Data\ItemData.json"
            |> File.ReadAllText
            |> Json.deserialize<GearBase list>
            |> List.filter (fun x -> Option.isSome x.Job)

        printfn "%A" data

    [<Fact>]
    let ParseSetDataTest () =
        let data =
            @"C:\Projects\DpmCalculator\DpmCalculator.Items\Data\SetData.json"
            |> File.ReadAllText
            |> Json.deserialize<SetBase list>

        printfn "%A" data