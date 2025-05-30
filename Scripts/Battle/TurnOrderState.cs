using Godot;
using System;

public partial class TurnOrderState : RefCounted
{
    [Signal] public delegate void ChangeStateEventHandler(TurnOrderState fromState, int toState);
    public TurnOrderStateMachine.TurnState state = TurnOrderStateMachine.TurnState.NONE;
    public TurnOrderStateMachine stateMachine = null;

    public TurnOrderState(TurnOrderStateMachine.TurnState initialState, TurnOrderStateMachine stateMachine)
    {
        this.state = initialState;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter(Character control)
    {
        GD.Print("Entering State: " + state);
        GD.Print(control.Name);
    }

    public virtual void Exit()
    {
        GD.Print("Exiting State: " + state);
    }
}

public partial class StartPowerUpState : TurnOrderState
{
    public StartPowerUpState(TurnOrderStateMachine.TurnState initialState, TurnOrderStateMachine stateMachine) : base(initialState, stateMachine)
    {
        this.state = initialState;
        this.stateMachine = stateMachine;
    }

    public override void Enter(Character character)
    {
        if (this.stateMachine.currentPhaseState == TurnOrderStateMachine.PhaseState.BATTLE_END) { return; }

        if(character is Player)
        {
            Player player = character as Player;
            player.StartOfTurnReset();
            
            RunManager.instance.currentRun.powerUpHandler.ActivatePowerUpsByType(PowerUp.ActivateType.START_OF_TURN, () => 
            {
                EmitSignal(TurnOrderState.SignalName.ChangeState, this, (int)TurnOrderStateMachine.TurnState.START_EFFECTS);
            });
        }
        else if (character is Enemy)
        {
            Enemy enemy = character as Enemy;
            enemy.StartOfTurnReset();
            
            EmitSignal(TurnOrderState.SignalName.ChangeState, this, (int)TurnOrderStateMachine.TurnState.START_EFFECTS);
        }
        else
        {
            GD.Print("Control node is invalid type");	
        }
    }

    public override void Exit()
    {
		
    }
}

public partial class StartEffectState : TurnOrderState
{
    public StartEffectState(TurnOrderStateMachine.TurnState initialState, TurnOrderStateMachine stateMachine) : base(initialState, stateMachine)
    {
        this.state = initialState;
        this.stateMachine = stateMachine;
    }

    public override void Enter(Character character)
    {
        if (this.stateMachine.currentPhaseState == TurnOrderStateMachine.PhaseState.BATTLE_END) { return; }
        
        character.statusHandler.ApplyStatusByType(Status.TriggerType.START_OF_TURN, () =>
        {
            EmitSignal(TurnOrderState.SignalName.ChangeState, this, (int)TurnOrderStateMachine.TurnState.CHAR_TURN);
        });
    }

    public override void Exit()
    {
        
    }
}

public partial class CharacterTurnState : TurnOrderState
{
    private Player player = null;
    private Enemy enemy = null;
    public CharacterTurnState(TurnOrderStateMachine.TurnState initialState, TurnOrderStateMachine stateMachine) : base(initialState, stateMachine)
    {
        this.state = initialState;
        this.stateMachine = stateMachine;
    }

    public override void Enter(Character character)
    {
        if (this.stateMachine.currentPhaseState == TurnOrderStateMachine.PhaseState.BATTLE_END) { return; }
        
        if (character.statusHandler.HasStatus(Status.StatusID.STUN))
        {
            GD.Print("Character is stunned!");
            EmitSignal(TurnOrderState.SignalName.ChangeState, this, (int)TurnOrderStateMachine.TurnState.END_POWERUPS);
        }
        
        if(character is Player)
        {
            this.player = character as Player;
            this.player.StartTurn(() =>
            {
                EmitSignal(TurnOrderState.SignalName.ChangeState, this, (int)TurnOrderStateMachine.TurnState.END_POWERUPS);
            });
        }
        else if (character is Enemy)
        {
            this.enemy = character as Enemy;
            this.enemy.TakeTurn(() => { EmitSignal(TurnOrderState.SignalName.ChangeState, this, (int)TurnOrderStateMachine.TurnState.END_POWERUPS); });
        }
        else
        {
            GD.Print("Control node is invalid type");	
        }
    }

    public override void Exit()
    {
        this.player = null;
        this.enemy = null;
    }
}

public partial class EndPowerUpState : TurnOrderState
{
    public EndPowerUpState(TurnOrderStateMachine.TurnState initialState, TurnOrderStateMachine stateMachine) : base(initialState, stateMachine)
    {
        this.state = initialState;
        this.stateMachine = stateMachine;
    }

    public override void Enter(Character character)
    {
        if (this.stateMachine.currentPhaseState == TurnOrderStateMachine.PhaseState.BATTLE_END) { return; }
        
        if(character is Player)
        {
            RunManager.instance.currentRun.powerUpHandler.ActivatePowerUpsByType(PowerUp.ActivateType.END_OF_TURN, () => 
            {
                EmitSignal(TurnOrderState.SignalName.ChangeState, this, (int)TurnOrderStateMachine.TurnState.END_EFFECTS);
            });
        }
        else if (character is Enemy)
        {
            Enemy enemy = character as Enemy;
            enemy.UpdateEnemy();
            enemy.UpdateUI();
            EmitSignal(TurnOrderState.SignalName.ChangeState, this, (int)TurnOrderStateMachine.TurnState.END_EFFECTS);
        }
        else
        {
            GD.Print("Control node is invalid type");	
        }
    }

    public override void Exit()
    {
		
    }
}

public partial class EndEffectState : TurnOrderState
{
    private Player player = null;
    private Enemy enemy = null;
	
    public EndEffectState(TurnOrderStateMachine.TurnState initialState, TurnOrderStateMachine stateMachine) : base(initialState, stateMachine)
    {
        this.state = initialState;
        this.stateMachine = stateMachine;
    }

    public override void Enter(Character character)
    {
        if (this.stateMachine.currentPhaseState == TurnOrderStateMachine.PhaseState.BATTLE_END) { return; }
        
        character.statusHandler.ApplyStatusByType(Status.TriggerType.END_OF_TURN, () =>
        {
            this.stateMachine.EndTurn();
            //EmitSignal(TurnOrderState.SignalName.ChangeState, this, (int)TurnOrderStateMachine.TurnState.CHAR_TURN);
        });
    }

    public override void Exit()
    {
        this.player = null;
        this.enemy = null;
    }
}
