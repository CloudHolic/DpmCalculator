namespace DpmCalculator.Core.Models

module StatType =
    type Stat = {
        Str: int
        StrRate: int

        Dex: int
        DexRate: int

        Int: int
        IntRate: int

        Luk: int
        LukRate: int

        MaxHp: int
        HpRate: int

        MaxMp: int
        MpRate: int

        PAtk: int
        PAtkRate: int

        MAtk: int
        MAtkRate: int

        ArmorIgnore: int
        BossDamage: int

        CritRate: int
        CritDamage: int

        FinalDamage: int
    }
