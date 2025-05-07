using Godot;
using System;
using System.Collections.Generic;

public partial class BattleView : UIView
{
    public Control playerSpawn { get; private set; } = null;
    public Player player { get; private set; } = null;
    public List<Control> enemySpawns { get; private set; } = new List<Control>();

    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        this.playerSpawn = this.GetNode<Control>("%PlayerSpawn");
        
        this.enemySpawns.Add(this.GetNode<Control>("%EnemySpawn"));
        this.enemySpawns.Add(this.GetNode<Control>("%EnemySpawn2"));
        this.enemySpawns.Add(this.GetNode<Control>("%EnemySpawn3"));
    }

    public void SetData(BattleData battleData)
    {
        this.player = ResourceManager.instance.playerScene.Instantiate() as Player;
        this.player.GetSceneNodes();
        this.player.SetPlayerData(RunManager.instance.currentRun.playerData);
        this.playerSpawn.AddChild(this.player);

        UIManager.instance.hudModel.SetPlayerData(player);
        
        for (int i = 0; i < battleData.enemyIDList.Length; i++)
        {
            Enemy enemy = ResourceManager.instance.enemyScene.Instantiate() as Enemy;
            enemy.GetSceneNodes();
            enemy.SetData(ResourceManager.instance.enemies[battleData.enemyIDList[i]]);
            this.enemySpawns[i].AddChild(enemy);
        }
    }

    public void Enter()
    {
        
    }
}
