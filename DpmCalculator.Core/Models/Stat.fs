namespace DpmCalculator.Core.Models

module Stat =
    type Stat = {
        Str: float
        StrRate: float

        Dex: float
        DexRate: float

        Int: float
        IntRate: float

        Luk: float
        LukRate: float

        MaxHp: float
        HpRate: float

        MaxMp: float
        MpRate: float

        PAtk: float
        PAtkRate: float

        MAtk: float
        MAtkRate: float

        ArmorIgnore: float
        BossDamage: float
        Damage: float

        CritRate: float
        CritDamage: float

        FinalDamage: float
    } with 
    static member Default = {
        Str = 0
        StrRate = 0

        Dex = 0
        DexRate = 0

        Int = 0
        IntRate = 0

        Luk = 0
        LukRate = 0

        MaxHp = 0
        HpRate = 0

        MaxMp = 0
        MpRate = 0

        PAtk = 0
        PAtkRate = 0

        MAtk = 0
        MAtkRate = 0

        ArmorIgnore = 0
        BossDamage = 0
        Damage = 0

        CritRate = 0
        CritDamage = 0

        FinalDamage = 0
    }

    member this.Add (other: Stat) = {
        Str = this.Str + other.Str
        StrRate = this.StrRate + other.StrRate

        Dex = this.Dex + other.Dex
        DexRate = this.DexRate + other.DexRate

        Int = this.Int + other.Int
        IntRate = this.IntRate + other.IntRate

        Luk = this.Luk + other.Luk
        LukRate = this.LukRate + other.LukRate

        MaxHp = this.MaxHp + other.MaxHp
        HpRate = this.HpRate + other.HpRate

        MaxMp = this.MaxMp + other.MaxMp
        MpRate = this.MpRate + other.MpRate

        PAtk = this.PAtk + other.PAtk
        PAtkRate = this.PAtkRate + other.PAtkRate

        MAtk = this.MAtk + other.MAtk
        MAtkRate = this.MAtkRate + other.MAtkRate

        ArmorIgnore = 100.0 - 0.01 * (100.0 - this.ArmorIgnore) * (100.0 - other.ArmorIgnore)
        BossDamage = this.BossDamage + other.BossDamage
        Damage = this.Damage + other.Damage

        CritRate = this.CritRate + other.CritRate
        CritDamage = this.CritDamage + other.CritDamage

        FinalDamage = this.FinalDamage + other.FinalDamage + this.FinalDamage * other.FinalDamage * 0.01
    }
        
    member this.Sub (other: Stat) = {
        Str = this.Str - other.Str
        StrRate = this.StrRate - other.StrRate

        Dex = this.Dex - other.Dex
        DexRate = this.DexRate - other.DexRate

        Int = this.Int - other.Int
        IntRate = this.IntRate - other.IntRate

        Luk = this.Luk - other.Luk
        LukRate = this.LukRate - other.LukRate

        MaxHp = this.MaxHp - other.MaxHp
        HpRate = this.HpRate - other.HpRate

        MaxMp = this.MaxMp - other.MaxMp
        MpRate = this.MpRate - other.MpRate

        PAtk = this.PAtk - other.PAtk
        PAtkRate = this.PAtkRate - other.PAtkRate

        MAtk = this.MAtk - other.MAtk
        MAtkRate = this.MAtkRate - other.MAtkRate

        ArmorIgnore = 100.0 - 100.0 * (100.0 - this.ArmorIgnore) / (100.0 - other.ArmorIgnore)
        BossDamage = this.BossDamage - other.BossDamage
        Damage = this.Damage - other.Damage

        CritRate = this.CritRate - other.CritRate
        CritDamage = this.CritDamage - other.CritDamage

        FinalDamage = (100.0 + this.FinalDamage) / (100.0 + other.FinalDamage) * 100.0 - 100.0
    }