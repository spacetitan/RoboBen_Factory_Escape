using Godot;
using System;
using System.Collections.Generic;

public partial class BattleModel : UIModel
{
    public Player player { get; set; } = null;
    public List<Enemy> enemies { get; set; } = new List<Enemy>();
    public List<Enemy> activeEnemies { get; set; } = new List<Enemy>();
    
    public BattleData battleData = null;
    public TurnOrderStateMachine turnOrderStateMachine = null;

    public override void _Ready()
    {
        ConnectEventSignals();
    }

    public void ConnectEventSignals()
    {
        EventManager.instance.PlayerDied += OnPlayerDeath;
        EventManager.instance.EnemyDied += OnEnemyDeath;
    }
    
    public void DisconnectEventSignals()
    {
        EventManager.instance.PlayerDied -= OnPlayerDeath;
        EventManager.instance.EnemyDied -= OnEnemyDeath;
    }

    public override void Enter()
    {
        this.turnOrderStateMachine = new TurnOrderStateMachine();
        
        BattleView view = this.views[ViewID.BATTLE] as BattleView;
        view.SetData(battleData);
        this.player = view.player;
        this.enemies = view.enemies;
        view.ShowView();
        
        this.turnOrderStateMachine.InitializeStateMachine(this.player, EndTurn);
    }

    public override void Exit()
    {
        foreach (KeyValuePair<ViewID, UIView> view in this.views)
        {
            view.Value.Exit();
        }
    }

    public void EndTurn()
    {
        if (this.turnOrderStateMachine.currentCharacter is Player)
        {
            foreach (Enemy enemy in enemies)
            {
                this.activeEnemies.Add(enemy);
            } 
            
            Enemy nextEnemy = this.activeEnemies[0];
            this.activeEnemies.Remove(nextEnemy);
            
            this.turnOrderStateMachine.ChangeCharacter(nextEnemy);
        }
        else if(this.activeEnemies.Count > 0)
        {
            Enemy enemy = this.activeEnemies[0];
            this.activeEnemies.Remove(enemy);
            
            this.turnOrderStateMachine.ChangeCharacter(enemy);
        }
        else
        {
            this.turnOrderStateMachine.ChangeCharacter(player);
        }
    }

    public void OnPlayerDeath()
    {
        this.turnOrderStateMachine.EndBattle(false, this.battleData);
    }

    public void OnEnemyDeath(Enemy enemy)
    {
        if (this.activeEnemies.Contains(enemy))
        {
            this.activeEnemies.Remove(enemy);
        }
        
        this.enemies.Remove(enemy);
        enemy.DestroyEnemy();

        if (this.enemies.Count <= 0)
        {
            this.turnOrderStateMachine.EndBattle(true, this.battleData);
        }
        
        if (this.turnOrderStateMachine.currentCharacter == enemy)
        {
            if (this.activeEnemies.Count > 0)
            {
                Enemy newEnemy = this.activeEnemies[0];
                this.activeEnemies.Remove(newEnemy);
            
                this.turnOrderStateMachine.ChangeCharacter(newEnemy);
            }
            else
            {
                this.turnOrderStateMachine.ChangeCharacter(player);
            }
        }
    }
}
