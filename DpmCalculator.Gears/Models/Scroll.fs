namespace DpmCalculator.Gears.Models

module Scroll =    
    open System
    open DpmCalculator.Core.Models.Stat
    open DpmCalculator.Core.Models.GearType

    type Scroll = {
        Name: string
        Type: GearEnum list
        Stat: Stat
    } with

    static member Default = {
        Name = String.Empty
        Type = [ ]
        Stat = Stat.Default
    }

    static member SpellTrace prob (gearType: GearEnum) statType level =
        let setStat (stat: int) (hp: int) (atk: int) =
            match statType with
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
        let (gearType, stat) =
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
                |> (fun x -> ([ GearEnum.Heart; GearEnum.Weapon; GearEnum.Katara ], x))

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
                |> (fun x -> ([ GearEnum.Gloves ], x))

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
                |> (fun x -> ([ GearEnum.Cap; GearEnum.Coat; GearEnum.LongCoat; GearEnum.Pants; GearEnum.Shoes; GearEnum.Cape; GearEnum.ShoulderPad; GearEnum.SubWeapon ], x))

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
                |> (fun x -> ([ GearEnum.FaceAccessory; GearEnum.EyeAccessory; GearEnum.Earrings; GearEnum.Ring; GearEnum.Pendant; GearEnum.Belt ], x))

            // Error
            | _ -> ([], Stat.Default)
                
        {
            Name = name
            Type = gearType
            Stat = stat
        }

    static member ChaosScroll str dex int luk hp mp patk matk =
        [str; dex; int; luk; hp / 10; mp / 10; patk; matk] 
        |> List.filter (fun x -> [0; 1; 2; 3; 4; 5] |> List.contains x)
        |> List.length
        |> function
            | 8 -> 
                let stat = { 
                    Stat.Default with
                        Str = str;
                        Dex = dex;
                        Int = int;
                        Luk = luk;
                        MaxHp = float hp;
                        MaxMp = float mp;
                        PAtk = patk;
                        MAtk = matk;
                }

                {
                    Name = "긍정의 혼돈 주문서"
                    Type = [ GearEnum.All ]
                    Stat = stat
                }
            | _ ->
                { 
                    Name = "Error"
                    Type = [ ]
                    Stat = Stat.Default
                }

    static member AmazingChaosScroll str dex int luk hp mp patk matk =
        [str; dex; int; luk; hp / 10; mp / 10; patk; matk] 
        |> List.filter (fun x -> [0; 1; 2; 3; 4; 6] |> List.contains x)
        |> List.length
        |> function
            | 8 -> 
                let stat = { 
                    Stat.Default with
                        Str = str;
                        Dex = dex;
                        Int = int;
                        Luk = luk;
                        MaxHp = float hp;
                        MaxMp = float mp;
                        PAtk = patk;
                        MAtk = matk;
                }

                {
                    Name = "놀라운 긍정의 혼돈 주문서"
                    Type = [ GearEnum.All ]
                    Stat = stat
                }
            | _ ->
                { 
                    Name = "Error"
                    Type = [ ]
                    Stat = Stat.Default
                }

    static member PartyQuestScroll magic =
        match magic with
        | true -> 
            {
                Name = "파티 퀘스트 한손무기 마력 주문서"
                Type = [ GearEnum.Heart; GearEnum.Weapon; GearEnum.Katara ]
                Stat = { Stat.Default with MAtk = 5 }
            }
        | false ->
            {
                Name = "파티 퀘스트 한손무기 공격력 주문서"
                Type = [ GearEnum.Heart; GearEnum.Weapon; GearEnum.Katara ]
                Stat = { Stat.Default with PAtk = 5 }
            }

    static member MagicalSpell magic atk =
        match atk with
        | a when a < 9 && a > 11 -> 
            {
                Name = "Error"
                Type = [ ]
                Stat = Stat.Default
            }
        | _ ->
            match magic with
            | true -> 
                {
                    Name = "매지컬 마력 주문서"
                    Type = [ GearEnum.Heart; GearEnum.Weapon; GearEnum.Katara ]
                    Stat = { Stat.Default with Str = 3; Dex = 3; Int = 3; Luk = 3; MAtk = atk }
                }
            | false ->
                {
                    Name = "매지컬 공격력 주문서"
                    Type = [ GearEnum.Heart; GearEnum.Weapon; GearEnum.Katara ]
                    Stat = { Stat.Default with Str = 3; Dex = 3; Int = 3; Luk = 3; PAtk = atk }
                }

    static member UltimateSpell =
        {
            Name = "얼티밋 방어구 강화 주문서"
            Type = [ GearEnum.Cap; GearEnum.Coat; GearEnum.LongCoat; GearEnum.Pants; GearEnum.Shoes; GearEnum.Cape; GearEnum.SubWeapon ]
            Stat = { Stat.Default with Str = 9; Dex = 9; Int = 9; Luk = 9 }
        }

    static member PrimeArmorSpell =
        {
            Name = "10주년 프라임 방어구 주문서"
            Type = [ GearEnum.Cap; GearEnum.Coat; GearEnum.LongCoat; GearEnum.Pants; GearEnum.Shoes; GearEnum.Cape; GearEnum.SubWeapon ]
            Stat = { Stat.Default with Str = 10; Dex = 10; Int = 10; Luk = 10 }
        }

    static member YggdrassilBlessing statType stat =
        match stat with
        | a when a < 5 && a > 15 -> 
            {
                Name = "Error"
                Type = [ ]
                Stat = Stat.Default
            }
        | _ ->
            let gearType = [ GearEnum.Cap; GearEnum.Coat; GearEnum.LongCoat; GearEnum.Pants; GearEnum.Shoes; GearEnum.Cape; GearEnum.SubWeapon ]
            match statType with
            | "str" -> 
                {
                    Name = "힘의 이그드라실의 축복"
                    Type = gearType
                    Stat = { Stat.Default with Str = stat }
                }
            | "dex" -> 
                {
                    Name = "민첩성의 이그드라실의 축복"
                    Type = gearType
                    Stat = { Stat.Default with Dex = stat }
                }
            | "int" -> 
                {
                    Name = "지력의 이그드라실의 축복"
                    Type = gearType
                    Stat = { Stat.Default with Int = stat }
                }
            | "luk" -> 
                {
                    Name = "행운의 이그드라실의 축복"
                    Type = gearType
                    Stat = { Stat.Default with Luk = stat }
                }
            | _ ->
                {
                    Name = "Error"
                    Type = [ ]
                    Stat = Stat.Default
                }

    static member MiracleArmorSpell magic atk =
        match atk with
        | a when a < 2 && a > 3 -> 
            {
                Name = "Error"
                Type = [ ]
                Stat = Stat.Default
            }
        | _ ->
            match magic with
            | true -> 
                {
                    Name = "미라클 방어구 마력 주문서 50%"
                    Type = [ GearEnum.Cap; GearEnum.Coat; GearEnum.LongCoat; GearEnum.Pants; GearEnum.Shoes; GearEnum.Cape; GearEnum.SubWeapon ]
                    Stat = { Stat.Default with MAtk = atk }
                }
            | false ->
                {
                    Name = "미라클 방어구 공격력 주문서 50%"
                    Type = [ GearEnum.Cap; GearEnum.Coat; GearEnum.LongCoat; GearEnum.Pants; GearEnum.Shoes; GearEnum.Cape; GearEnum.SubWeapon ]
                    Stat = { Stat.Default with PAtk = atk }
                }

    static member PrimeAccessorySpell =
        {
            Name = "10주년 프라임 악세서리 주문서"
            Type = [ GearEnum.FaceAccessory; GearEnum.EyeAccessory; GearEnum.Earrings; GearEnum.Ring; GearEnum.Pendant; GearEnum.Belt ]
            Stat = { Stat.Default with Str = 10; Dex = 10; Int = 10; Luk = 10 }
        }

    static member MiracleAccessorySpell magic atk =
        match atk with
        | a when a < 0 && a > 4 -> 
            {
                Name = "Error"
                Type = [ ]
                Stat = Stat.Default
            }
        | _ ->
            match magic with
            | true -> 
                {
                    Name = "미라클 악세서리 마력 주문서 50%"
                    Type = [ GearEnum.FaceAccessory; GearEnum.EyeAccessory; GearEnum.Earrings; GearEnum.Ring; GearEnum.Pendant; GearEnum.Belt ]
                    Stat = { Stat.Default with MAtk = atk }
                }
            | false ->
                {
                    Name = "미라클 악세서리 공격력 주문서 50%"
                    Type = [ GearEnum.FaceAccessory; GearEnum.EyeAccessory; GearEnum.Earrings; GearEnum.Ring; GearEnum.Pendant; GearEnum.Belt ]
                    Stat = { Stat.Default with PAtk = atk }
                }

    static member AccessoryScroll magic atk =
        match atk with
        | a when a < 2 && a > 4 -> 
            {
                Name = "Error"
                Type = [ ]
                Stat = Stat.Default
            }
        | _ ->
            match magic with
            | true -> 
                {
                    Name = "악세서리 마력 스크롤"
                    Type = [ GearEnum.FaceAccessory; GearEnum.EyeAccessory; GearEnum.Earrings; GearEnum.Ring; GearEnum.Pendant; GearEnum.Belt ]
                    Stat = { Stat.Default with MAtk = atk }
                }
            | false ->
                {
                    Name = "악세서리 공격력 스크롤"
                    Type = [ GearEnum.FaceAccessory; GearEnum.EyeAccessory; GearEnum.Earrings; GearEnum.Ring; GearEnum.Pendant; GearEnum.Belt ]
                    Stat = { Stat.Default with PAtk = atk }
                }

    static member PremiumAccessoryScroll magic atk =
        match atk with
        | a when a < 4 && a > 5 -> 
            {
                Name = "Error"
                Type = [ ]
                Stat = Stat.Default
            }
        | _ ->
            match magic with
            | true -> 
                {
                    Name = "프리미엄 악세서리 마력 스크롤"
                    Type = [ GearEnum.FaceAccessory; GearEnum.EyeAccessory; GearEnum.Earrings; GearEnum.Ring; GearEnum.Pendant; GearEnum.Belt ]
                    Stat = { Stat.Default with MAtk = atk }
                }
            | false ->
                {
                    Name = "프리미엄 악세서리 공격력 스크롤"
                    Type = [ GearEnum.FaceAccessory; GearEnum.EyeAccessory; GearEnum.Earrings; GearEnum.Ring; GearEnum.Pendant; GearEnum.Belt ]
                    Stat = { Stat.Default with PAtk = atk }
                }

    static member EarringMagicSpell =
        {
            Name = "귀 장식 지력 주문서 10%"
            Type = [ GearEnum.Earrings ]
            Stat = { Stat.Default with Int = 3; MAtk = 5 }
        }

    static member MiraclePetEquipSpell magic atk =
        match atk with
        | a when a < 0 && a > 4 -> 
            {
                Name = "Error"
                Type = [ ]
                Stat = Stat.Default
            }
        | _ ->
            match magic with
            | true -> 
                {
                    Name = "미라클 펫장비 마력 주문서 50%"
                    Type = [ GearEnum.PetEquip ]
                    Stat = { Stat.Default with MAtk = atk }
                }
            | false ->
                {
                    Name = "미라클 펫장비 공격력 주문서 50%"
                    Type = [ GearEnum.PetEquip ]
                    Stat = { Stat.Default with PAtk = atk }
                }

    static member PetEquipScroll magic atk =
        match atk with
        | a when a < 2 && a > 4 -> 
            {
                Name = "Error"
                Type = [ ]
                Stat = Stat.Default
            }
        | _ ->
            match magic with
            | true -> 
                {
                    Name = "펫장비 마력 스크롤"
                    Type = [ GearEnum.PetEquip ]
                    Stat = { Stat.Default with MAtk = atk }
                }
            | false ->
                {
                    Name = "펫장비 공격력 스크롤"
                    Type = [ GearEnum.PetEquip ]
                    Stat = { Stat.Default with PAtk = atk }
                }

    static member PremiumPetEquipScroll magic atk =
        match atk with
        | a when a < 4 && a > 5 -> 
            {
                Name = "Error"
                Type = [ ]
                Stat = Stat.Default
            }
        | _ ->
            match magic with
            | true -> 
                {
                    Name = "프리미엄 펫장비 마력 스크롤"
                    Type = [ GearEnum.PetEquip ]
                    Stat = { Stat.Default with MAtk = atk }
                }
            | false ->
                {
                    Name = "프리미엄 펫장비 공격력 스크롤"
                    Type = [ GearEnum.PetEquip ]
                    Stat = { Stat.Default with PAtk = atk }
                }
