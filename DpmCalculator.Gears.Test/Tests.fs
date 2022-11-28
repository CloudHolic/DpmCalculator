namespace DpmCalculator.Items.Test

module Tests =
    open System.IO
    open Xunit
    open FSharp.Json
    open DpmCalculator.Gears.Gear
    open DpmCalculator.Gears.SetEffect

    [<Fact>]
    let ParseGearDataTest () =
        let data =
            @"C:\Projects\DpmCalculator\DpmCalculator.Gears\Data\GearData.json"
            |> File.ReadAllText
            |> Json.deserialize<GearBase list>
            |> List.filter (fun x -> Option.isSome x.Job)

        printfn "%A" data

    [<Fact>]
    let ParseSetDataTest () =
        let data =
            @"C:\Projects\DpmCalculator\DpmCalculator.Gears\Data\SetData.json"
            |> File.ReadAllText
            |> Json.deserialize<SetBase list>

        printfn "%A" data