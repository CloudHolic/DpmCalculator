namespace DpmCalculator.Items

module SetEffect =
    open FSharp.Json

    type EffectBase = {
        [<JsonField("count")>]
        Count: int

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
                
        [<JsonField("crit_damage", DefaultValue = 0)>]
        CritDamage: int        
    }

    type SetEffectBase = {
        [<JsonField("name")>]
        Name: string
        
        [<JsonField("set_code")>]
        SetCode: int

        [<JsonField("item_code")>]
        ItemCode: int list

        [<JsonField("effect")>]
        Effects: EffectBase list
    }