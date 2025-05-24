using Godot;
using System;

[GlobalClass]
public partial class BattleData : Resource
{
    public enum BattleID { NONE, GROUP1, GROUP2, GROUP3, GROUP4, GROUP5, GROUP6, GROUP7, GROUP8, BOSS1, }
    [Export] public BattleID battleID { get; private set; } = BattleID.NONE;
    [Export] public EnemyData[] enemyList { get; private set; } = null;
    [Export] public int tier { get; private set; } = 0;
    [Export] public float weight { get; private set; } = 0.0f;
    [Export] public int money { get; private set; } = 0;
    public float accumilatedWeight = 0.0f;
    
    public void SetData(BattleData battleData)
    {
        this.battleID = battleData.battleID;
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
