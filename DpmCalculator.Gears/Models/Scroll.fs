namespace DpmCalculator.Gears.Models

module Scroll =    
    open System
    open DpmCalculator.Core.Models.Stat

    type Scroll = {
        Name: string
        Stat: Stat
    } with

    static member Default = {
        Name = String.Empty
        Stat = Stat.Default
    }

