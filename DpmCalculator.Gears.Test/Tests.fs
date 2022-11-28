namespace DpmCalculator.Items.Test

module Tests =
    open System
    open System.IO
    open Xunit
    open FSharp.Json
    open DpmCalculator.Gears.Models.Gear
    open DpmCalculator.Gears.Models.SetEffect
    open DpmCalculator.Gears.GearBuilder

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

    [<Fact>]
    let SearchIdTest () =
        let id = 
            "앱솔랩스 메이지"
            |> searchIdByName false
        
        Assert.True(List.contains 1152176 id)