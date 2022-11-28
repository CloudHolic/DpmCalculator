namespace DpmCalculator.Gears

module Core =
    open System.IO
    open System.Reflection
    open FSharp.Json
    open Models.Gear
    open Models.SetEffect

    let assembly = Assembly.GetExecutingAssembly ()
    let gearStream = "DpmCalculator.Gears.Data.GearData.json" |> assembly.GetManifestResourceStream
    let setStream = "DpmCalculator.Gears.Data.SetData.json" |> assembly.GetManifestResourceStream
    let gearReader = new StreamReader(gearStream)
    let setReader = new StreamReader(setStream)

    let gearData = gearReader.ReadToEnd () |> Json.deserialize<GearBase list>
    let setData = setReader.ReadToEnd () |> Json.deserialize<SetBase list>