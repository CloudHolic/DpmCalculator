namespace DpmCalculator.Items

module Item =
    open FSharp.Json
    open DpmCalculator.Core.Models.ItemType
    open DpmCalculator.Core.Models.JobType

    type ItemBase = {
        [<JsonField("name")>]
        Name: string

        [<JsonField("item_code")>]
        ItemCode: int

        [<JsonField("set_code", DefaultValue = 0)>]
        SetCode: int

        [<JsonField("type", EnumValue = EnumMode.Value)>]
        Type: ItemEnum
        
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