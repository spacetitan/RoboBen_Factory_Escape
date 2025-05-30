using Godot;
using System;
using System.Collections.Generic;
using System.Numerics;
using Vector2 = Godot.Vector2;

public partial class BattleView : UIView
{
    public Control playerSpawn { get; private set; } = null;
    public Player player { get; private set; } = null;
    public List<Enemy> enemies { get; private set; } = new List<Enemy>();
    public List<Control> enemySpawns { get; private set; } = new List<Control>();
    
    public Vector2 playerSize { get; private set; } = Vector2.Zero;
    public Vector2 EnemySize { get; private set; } = Vector2.Zero;

    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        this.playerSpawn = this.GetNode<Control>("%PlayerSpawn");
        this.playerSize = new Vector2(this.playerSpawn.Size.X, this.playerSpawn.Size.X);
        
        this.enemySpawns.Add(this.GetNode<Control>("%EnemySpawn"));
        this.enemySpawns.Add(this.GetNode<Control>("%EnemySpawn2"));
        this.enemySpawns.Add(this.GetNode<Control>("%EnemySpawn3"));
        this.EnemySize = new Vector2(this.enemySpawns[0].Size.X, this.enemySpawns[0].Size.X);
    }

    public void SetData(BattleData battleData)
    {
        this.player = ResourceManager.instance.playerScene.Instantiate() as Player;
        this.player.GetSceneNodes();
        this.player.SetCustomMinimumSize(this.playerSize);
        this.playerSpawn.AddChild(this.player);
        this.player.SetPlayerData(RunManager.instance.currentRun);

        UIManager.instance.hudModel.SetPlayerData(player);
        
        for (int i = 0; i < battleData.enemyList.Length; i++)
        {
            Enemy enemy = ResourceManager.instance.enemyScene.Instantiate() as Enemy;
            enemy.GetSceneNodes();
            enemy.SetCustomMinimumSize(this.EnemySize);
            this.enemySpawns[i].AddChild(enemy);
            enemy.SetData(battleData.enemyList[i].CreateInstance());
            this.enemies.Add(enemy);
        }
    }

    public override void Enter()
    {
        UIManager.instance.vfxModel.OpenCurtain();
    }

    public override void Exit()
    {
        if (this.player != null)
        {
            this.player.DestroyPlayer();
        }
        
        foreach (Enemy enemy in this.enemies)
        {
            if (enemy != null)
            {
                enemy.DestroyEnemy();
            }
        }

        foreach (Control enemySpawn in this.enemySpawns)
        {
            if (enemySpawn.GetChildren().Count > 0)
            {
                Enemy enemy = enemySpawn.GetChild(0) as Enemy;
                enemy.DestroyEnemy();
            }
        }
    }
}
