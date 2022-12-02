namespace DpmCalculator.Gears.Models

module Gear =
    open FSharp.Json
    open DpmCalculator.Core.Models.Job
    open DpmCalculator.Core.Models.Stat
    open AdditionalType
    open GearType
    open Scroll
        
    type JobUnion = 
        | JobBranch of JobBranchEnum
        | JobClass of JobClassEnum
        | Job of JobEnum list
        | All

    type GearBase = {
        [<JsonField("name")>]
        Name: string

        [<JsonField("item_code")>]
        ItemCode: int

        [<JsonField("set_code", DefaultValue = 0)>]
        SetCode: int

        [<JsonField("type", EnumValue = EnumMode.Value)>]
        Type: GearEnum
        
        [<JsonField("job_branch", EnumValue = EnumMode.Value, DefaultValue = JobBranchEnum.All)>]
        JobBranch: JobBranchEnum
        
        [<JsonField("job_field", EnumValue = EnumMode.Value, DefaultValue = JobClassEnum.All)>]
        JobClass: JobClassEnum
        
        [<JsonField("job")>]
        Job: int list option

        [<JsonField("joker", DefaultValue = false)>]
        Joker: bool

        [<JsonField("block_star", DefaultValue = false)>]
        BlockStar: bool

        [<JsonField("upgrade_count", DefaultValue = 0)>]
        UpgradeCount: int

        [<JsonField("block_hammer", DefaultValue = false)>]
        BlockHammer: bool

        [<JsonField("level", DefaultValue = 0)>]
        Level: int
        
        [<JsonField("str", DefaultValue = 0)>]
        Str: int

        [<JsonField("dex", DefaultValue = 0)>]
        Dex: int
                
        [<JsonField("int", DefaultValue = 0)>]
        Int: int
        
        [<JsonField("luk", DefaultValue = 0)>]
        Luk: int
        
        [<JsonField("max_hp", DefaultValue = 0)>]
        MaxHp: int
        
        [<JsonField("max_mp", DefaultValue = 0)>]
        MaxMp: int
        
        [<JsonField("hp_rate", DefaultValue = 0)>]
        HpRate: int
        
        [<JsonField("mp_rate", DefaultValue = 0)>]
        MpRate: int
        
        [<JsonField("patk", DefaultValue = 0)>]
        PAtk: int
        
        [<JsonField("matk", DefaultValue = 0)>]
        MAtk: int
        
        [<JsonField("armor_ignore", DefaultValue = 0)>]
        ArmorIgnore: int
        
        [<JsonField("boss_damage", DefaultValue = 0)>]
        BossDamage: int
        
        [<JsonField("crit_rate", DefaultValue = 0)>]
        CritRate: int
        
        [<JsonField("crit_damage", DefaultValue = 0)>]
        CritDamage: int
        
        [<JsonField("final_damage", DefaultValue = 0)>]
        FinalDamage: int
    }
        
    type Gear(gearBase: GearBase) =
        member _.Name = gearBase.Name
        member _.ItemCode = gearBase.ItemCode
        member _.SetCode = gearBase.SetCode

        member _.Type = gearBase.Type
        member _.Job = 
            match (gearBase.JobBranch, gearBase.JobClass, gearBase.Job) with
            | (JobBranchEnum.All, JobClassEnum.All, None) -> All
            | (JobBranchEnum.All, JobClassEnum.All, Some [ ]) -> All
            | (JobBranchEnum.All, JobClassEnum.All, Some jobList) -> Job (jobList |> List.map enum<JobEnum>)
            | (JobBranchEnum.All, jobClass, _) -> JobClass jobClass
            | (jobBranch, JobClassEnum.All, _) -> JobBranch jobBranch
            | (_, _, _) -> All

        member _.Level = gearBase.Level
        member _.Joker = gearBase.Joker

        member _.MaxUpgrade = if gearBase.BlockHammer then 0 else 1 |> (+) gearBase.UpgradeCount

        member this.Superior = [ 1132174; 1132175; 1132176; 1132177; 1132178 ] |> List.contains this.ItemCode
        member this.MaxStar =
            if gearBase.BlockStar then 0
            else
                match (this.Level, this.Superior) with
                | (_, true) -> 15
                | (l, _) when l < 95 -> 5
                | (l, _) when l >= 95 && l < 107 -> 8
                | (l, _) when l >= 108 && l < 117 -> 10            
                | (l, _) when l >= 118 && l < 127 -> 15
                | (l, _) when l >= 128 && l < 137 -> 20
                | _ -> 25

        member _.BaseStat = {
            Str = gearBase.Str
            StrRate = 0

            Dex = gearBase.Dex
            DexRate = 0

            Int = gearBase.Int
            IntRate = 0

            Luk = gearBase.Luk
            LukRate = 0

            MaxHp = gearBase.MaxHp
            HpRate = gearBase.HpRate

            MaxMp = gearBase.MaxMp
            MpRate = gearBase.MpRate

            PAtk = gearBase.PAtk
            PAtkRate = 0

            MAtk = gearBase.MAtk
            MAtkRate = 0

            ArmorIgnore = gearBase.ArmorIgnore
            BossDamage = gearBase.BossDamage
            Damage = 0

            CritRate = gearBase.CritRate
            CritDamage = gearBase.CritDamage

            FinalDamage = gearBase.FinalDamage
        }
                
        member val CurrentUpgrade = 0 with get, set
        member val CurrentStar = 0 with get, set

        member val AdditionalStat = Stat.Default with get, set
        member val UpgradeStat = Stat.Default with get, set
        member val PotentialStat = Stat.Default with get, set
        member val AdditionalPotentialStat = Stat.Default with get, set

        member this.ApplyAdditionalStat (statType: AdditionalEnum) grade =
            this.AdditionalStat <-
                match int statType with
                | t when t <= 4 -> 
                    let bonusStat = 
                        if this.Level = 250 then 220 else this.Level / 10 * 10
                        |> (fun x -> x / 20 + 1)
                        |> (*) grade
                    match t with
                    | 1 -> { Stat.Default with Str = bonusStat }
                    | 2 -> { Stat.Default with Dex = bonusStat }
                    | 3 -> { Stat.Default with Int = bonusStat }
                    | 4 -> { Stat.Default with Luk = bonusStat }
                    | _ -> Stat.Default

                | t when t >= 5 && t <= 10 -> 
                    let bonusStat =
                        if this.Level = 250 then 220 else this.Level / 10 * 10
                        |> (fun x -> x / 40 + 1)
                        |> (*) grade
                    match t with
                    | 5 -> { Stat.Default with Str = bonusStat; Dex = bonusStat }
                    | 6 -> { Stat.Default with Str = bonusStat; Int = bonusStat }
                    | 7 -> { Stat.Default with Str = bonusStat; Luk = bonusStat }
                    | 8 -> { Stat.Default with Dex = bonusStat; Int = bonusStat }
                    | 9 -> { Stat.Default with Dex = bonusStat; Luk = bonusStat }
                    | 10 -> { Stat.Default with Int = bonusStat; Luk = bonusStat }
                    | _ -> Stat.Default
                    
                | t when t >= 11 && t <= 12 ->
                    let bonusStat = 
                        if this.Level = 250 then 700 else this.Level / 10 * 30
                        |> (*) grade
                    match t with
                    | 11 -> { Stat.Default with MaxHp = bonusStat }
                    | 12 -> { Stat.Default with MaxMp = bonusStat }
                    | _ -> Stat.Default

                | t when t >= 13 && t <= 14 -> 
                    if this.Type = GearEnum.Weapon then
                        let bonusRate =
                            (this.Level / 40) + 1
                            |> (*) grade
                            |> (fun x -> float x * 1.1 ** float (grade - 3))
                        match t with
                        | 13 -> { Stat.Default with PAtk = this.BaseStat.PAtk * bonusRate / 100.0 }
                        | 14 -> { Stat.Default with MAtk = this.BaseStat.MAtk * bonusRate / 100.0 }
                        | _ -> Stat.Default
                    else
                        match t with
                        | 13 -> { Stat.Default with PAtk = grade }
                        | 14 -> { Stat.Default with MAtk = grade }
                        | _ -> Stat.Default

                | 15 -> { Stat.Default with BossDamage = 2 * grade |> float }
                | 16 -> { Stat.Default with Damage = grade }
                | 17 -> { Stat.Default with StrRate = grade; DexRate = grade; IntRate = grade; LukRate = grade }
                | _ -> Stat.Default
                |> this.AdditionalStat.Add

        member this.ApplyUpgrade (scroll: Scroll) count =
            let scrollStat = 
                if List.contains this.Type scroll.Type 
                then scroll.Stat 
                else Stat.Default
                        
            this.UpgradeStat <- 
                ((this.MaxUpgrade - this.CurrentUpgrade, count) ||> min, (fun _ -> scrollStat))
                ||> List.init
                |> List.reduce (fun x y -> x.Add y)
                |> this.UpgradeStat.Add

        member _.ApplyStarForce star amazing bonus =
            0

        member _.ApplyPotential =
            0

        member _.ApplyAdditionalPotential =
            0

        member this.TotalStat () = 
            this.BaseStat
            |> this.AdditionalStat.Add
            |> this.UpgradeStat.Add
            |> this.PotentialStat.Add
            |> this.AdditionalPotentialStat.Add