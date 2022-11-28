namespace DpmCalculator.Gears

module GearBuilder =
    open System.IO
    open System.Reflection
    open FSharp.Json
    open Models.Gear
    open Models.SetEffect
    
    let private baseResource = "DpmCalculator.Gears.Data."

    let private gearData =
        let assembly = Assembly.GetExecutingAssembly ()
        use stream = baseResource + "GearData.json" |> assembly.GetManifestResourceStream
        use reader = new StreamReader(stream)

        reader.ReadToEnd () 
        |> Json.deserialize<GearBase list>

    let private setData =
        let assembly = Assembly.GetExecutingAssembly ()
        use stream = baseResource + "SetData.json" |> assembly.GetManifestResourceStream
        use reader = new StreamReader(stream)

        reader.ReadToEnd () 
        |> Json.deserialize<SetBase list>

    //let private createGear gearBase = new Gear ()

    let searchIdByName exact name =
        gearData
        |> List.filter (fun x ->
            match exact with
            | true -> x.Name = name
            | false -> x.Name.Contains name)
        |> List.map (fun x -> x.ItemCode)

    let createGearById id =
        gearData
        |> List.find (fun x -> x.ItemCode = id)
        