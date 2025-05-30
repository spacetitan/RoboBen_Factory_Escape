using Godot;
using System;

public partial class RunStats
{
    public int enemySlain = 0;
    public int moneySpent = 0;
    public int reRollsUsed = 0;

    public Godot.Collections.Dictionary<StringName, Variant> saveStats()
    {
        Godot.Collections.Dictionary<StringName, Variant> runData = new Godot.Collections.Dictionary<StringName, Variant>()
        {
            {"Enemies slain", this.enemySlain},
            {"Money Spent", this.moneySpent},
            {"Re-Rolls Used", this.reRollsUsed},
        };
        
        return runData;
    }

    public void loadStats(Godot.Collections.Dictionary stats)
    {
        this.enemySlain = (int)stats["Enemies slain"];
        this.moneySpent = (int)stats["Money Spent"];
        this.reRollsUsed = (int)stats["Re-Rolls Used"];
    }
}
