using Godot;
using System;

[GlobalClass]
public partial class BattleData : Resource
{
    public enum BattleID
    {
        NONE,
        GROUP1,
        GROUP2,
        GROUP3,
        BOSS1,
    }
    [Export] public BattleID battleID = BattleID.NONE;
    [Export] public EnemyData[] enemyList;
    [Export] public int tier = 0;
    [Export] public float weight = 0.0f;
    [Export] public int money = 0;
    public float accumilatedWeight = 0.0f;
    
    public void SetData(BattleData battleData)
    {
        this.tier = battleData.tier;
        this.weight = battleData.weight;
        this.money = battleData.money;

        this.enemyList = new EnemyData[battleData.enemyList.Length];
        for (int i = 0; i < battleData.enemyList.Length; i++)
        {
            this.enemyList[i] = battleData.enemyList[i];
        }
        
        this.accumilatedWeight = battleData.accumilatedWeight;
    }
    
    public Godot.Collections.Dictionary<StringName, Variant> SaveBattleData()
    {
        Godot.Collections.Dictionary<StringName, Variant> data = new Godot.Collections.Dictionary<StringName, Variant>()
        {
            {"BattleID", (int) this.battleID},
        };
        
        return data;
    }
}
