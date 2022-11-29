namespace DpmCalculator.Gears.Models

module Scroll =    
    open System
    open DpmCalculator.Core.Models.Stat
    open DpmCalculator.Core.Models.GearType

    type Scroll = {
        Name: string
        Stat: Stat
    } with

    static member Default = {
        Name = String.Empty
        Stat = Stat.Default
    }

    static member SpellTrace prob (gearType: GearEnum) mainStat level =
        let setStat (stat: int) (hp: int) (atk: int) =
            match mainStat with
            | "str" -> { Stat.Default with Str = stat; MaxHp = hp; PAtk = atk }
            | "dex" -> { Stat.Default with Dex = stat; MaxHp = hp; PAtk = atk }
            | "int" -> { Stat.Default with Int = stat; MaxHp = hp; MAtk = atk }
            | "luk" -> { Stat.Default with Luk = stat; MaxHp = hp; PAtk = atk }
            | "hp" -> { Stat.Default with MaxHp = (stat * 50 + hp) |> float; PAtk = atk }
            | "all" -> 
                let half = stat / 2
                {Stat.Default with Str = half; Dex = half; Luk = half; MaxHp = hp; PAtk = atk }
            | _ -> Stat.Default

        let name = "주문의 흔적" + string prob + "%"
        let stat =
            match int gearType with
            // Heart / Waepon / Katara
            | 19 | 21 | 22 -> 
                let probGrade = match prob with | 100 -> 1 | 70 -> 2 | 30 -> 3 | 15 -> 4 | _ -> 0
                let levelGrade = 
                    match level with
                    | l when l <= 70 -> 1
                    | l when l >= 80 && l <= 110 -> 2
                    | l when l >= 120 -> 3
                    | _ -> 0

                match (probGrade, levelGrade) with
                | (0, _) | (_, 0) -> (0, 0)
                | (p, l) ->
                    match p + l with
                    | 2 -> (0, 1) | 3 -> (0, 2) | 4 -> (1, 3)
                    | 5 -> (2, 5) | 6 -> (3, 7) | 7 -> (4, 9) | _ -> (0, 0)
                |> (fun (x, y) -> (x, 0, y))
                |||> setStat

            // Gloves
            | 9 -> 
                let probGrade = match prob with | 100 -> 1 | 70 -> 2 | 30 -> 3 | _ -> 0
                let levelGrade = 
                    match level with
                    | l when l <= 70 -> 1
                    | l when l >= 75 -> 2
                    | _ -> 0

                match (probGrade, levelGrade) with
                | (0, _) | (_, 0) -> 0
                | (p, l) -> match p + l with | 3 -> 1 | 4 -> 2 | 5 -> 3 | _ -> 0
                |> (fun x -> (0, 0, x))
                |||> setStat

            // Armor (Cap, Coat, LongCoat, Pants, Shoes, Cape, ShoulderPad, SubWeapon(Shield))
            | 1 | 5 | 6 | 7 | 8 | 10 | 16 | 23 ->  
                let probGrade = match prob with | 100 -> 1 | 70 -> 2 | 30 -> 3 | _ -> 0
                let levelGrade = 
                    match level with
                    | l when l <= 70 -> 1
                    | l when l >= 75 && l <= 110 -> 2
                    | l when l >= 120 -> 3
                    | _ -> 0

                match (probGrade, levelGrade) with
                | (1, l) -> match l with | 1 -> (1, 5) | 2 -> (2, 20) | 3 -> (3, 30) | _ -> (0, 0)
                | (2, l) -> match l with | 1 -> (2, 15) | 2 -> (3, 40) | 3 -> (4, 70) | _ -> (0, 0)
                | (3, l) -> match l with | 1 -> (3, 30) | 2 -> (5, 70) | 3 -> (7, 120) | _ -> (0, 0)
                | _ -> (0, 0)
                |> (fun (x, y) -> (x, y, 0))
                |||> setStat

            // Accessory (FaceAccessory, EyeAccessory, Earrings, Ring, Pendant, Belt)
            | 2 | 3 | 4 | 11 | 13 | 14 ->
                let probGrade = match prob with | 100 -> 1 | 70 -> 2 | 30 -> 3 | _ -> 0
                let levelGrade = 
                    match level with
                    | l when l <= 70 -> 1
                    | l when l >= 75 && l <= 110 -> 2
                    | l when l >= 120 -> 3
                    | _ -> 0

                match (probGrade, levelGrade) with
                | (1, l) -> match l with | 1 -> 1 | 2 -> 1 | 3 -> 2 | _ -> 0
                | (2, l) -> match l with | 1 -> 2 | 2 -> 2 | 3 -> 3 | _ -> 0
                | (3, l) -> match l with | 1 -> 3 | 2 -> 4 | 3 -> 5 | _ -> 0
                | _ -> 0
                |> (fun x -> (x, 0, 0))
                |||> setStat

            // Error
            | _ -> Stat.Default
                
        {
            Name = name
            Stat = stat
        }