using Godot;
using System;

[GlobalClass]
public partial class BattleData : Resource
{
    [Export] public string[] enemyIDList;
    [Export] public int tier = 0;
    [Export] public float weight = 0.0f;
    [Export] public int money = 0;
    public float accumilatedWeight = 0.0f;
    
    public void SetData(BattleData battleData)
    {
        this.tier = battleData.tier;
        this.weight = battleData.weight;

        this.enemyIDList = new String[battleData.enemyIDList.Length];
        for (int i = 0; i < battleData.enemyIDList.Length; i++)
        {
            this.enemyIDList[i] = battleData.enemyIDList[i];
        }
        
        this.accumilatedWeight = battleData.accumilatedWeight;
    }
    
    public Godot.Collections.Dictionary<StringName, Variant> SaveBattleData()
    {
        Godot.Collections.Dictionary<StringName, Variant> data = new Godot.Collections.Dictionary<StringName, Variant>()
        {
            {"tier", this.tier},
            {"weight", this.weight},
            {"money", this.money},
        };

        for (int i = 0; i < this.enemyIDList.Length; i++)
        {
            data.Add("Enemy ID " + i, enemyIDList[i]);
        }
        
        return data;
    }
}
