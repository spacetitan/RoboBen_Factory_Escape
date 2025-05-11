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

    public override void Enter()
    {
        this.turnOrderStateMachine = new TurnOrderStateMachine();
        
        BattleView view = this.views["battle"] as BattleView;
        view.SetData(battleData);
        this.player = view.player;
        this.enemies = view.enemies;
        view.ShowView();
        
        this.turnOrderStateMachine.InitializeStateMachine(this.player, EndTurn);
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
}
