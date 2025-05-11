using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public partial class TurnOrderStateMachine : Node
{
    public enum PhaseState { NONE, BATTLE_START, BATTLE_END, PLAYER, ENEMY}
    public enum TurnState { NONE, START_POWERUPS, END_POWERUPS, START_EFFECTS, END_EFFECTS, CHAR_TURN}
    
    [Export] public TurnOrderState initialState = null;
    [Export] public PhaseState currentPhaseState { get; private set; } = PhaseState.BATTLE_START;
    private TurnOrderState currentState = null;
    private List<TurnOrderState> flowStates = new List<TurnOrderState>();
    public Character currentCharacter { get; private set; } = null;
    
    public Action OnTurnEnd = null;
    
    public void InitializeStateMachine(Character StartingCharacter, Action OnTurnEnd = null)
    {
        if(!this.flowStates.Any())
        {
            this.flowStates.Add(new StartPowerUpState(TurnState.START_POWERUPS, this));
            this.flowStates.Add(new StartEffectState(TurnState.START_EFFECTS, this));
            this.flowStates.Add(new CharacterTurnState(TurnState.CHAR_TURN, this));
            this.flowStates.Add(new EndPowerUpState(TurnState.END_POWERUPS, this));
            this.flowStates.Add(new EndEffectState(TurnState.END_EFFECTS, this));
			
            foreach(TurnOrderState states in flowStates)
            {
                states.ChangeState += OnStateChanged;
            }
        }
        
        this.OnTurnEnd = OnTurnEnd;

        if(StartingCharacter is Player)
        {
            ChangeCharacter(StartingCharacter as Player);
        }
        else if(StartingCharacter is Enemy)
        {
            ChangeCharacter(StartingCharacter as Enemy);
        }
        else
        {
            GD.Print("Current character is invalid.");
        }
    }
    
    private void OnStateChanged(TurnOrderState from, int to)
    {
        if(this.currentPhaseState == PhaseState.BATTLE_END)
        {
            //GD.Print("Changing to end battle state");
            return;
        }

        if(from != this.currentState)
        {
            GD.Print("The current state does not match. From: " + from.state + " =/= " + this.currentState.state);
            return;
        }

        TurnOrderState newstate = flowStates.Find(x => x.state == (TurnState)to);
        if(newstate == null)
        {
            GD.Print("The new state cannot be found. State: " + (TurnState)to);
            return;
        }

        //GD.Print(from.state.ToString() + " -> " + ((TurnState)to).ToString());

        if(this.currentState != null)
        {
            this.currentState.Exit();
        }

        currentState = newstate;
        newstate.Enter(this.currentCharacter);
    }
    
    public void ChangeCharacter(Character character)
    {
        if(character is Player)
        {
            this.currentPhaseState = PhaseState.PLAYER;
            this.currentCharacter = character;
        }
        else if(character is Enemy)
        {
            this.currentPhaseState = PhaseState.ENEMY;
            this.currentCharacter = character;
        }
        else
        {
            GD.Print("Current character is invalid.");
        }
		
        if (this.currentState != null)
        {
            this.currentState.EmitSignal(TurnOrderState.SignalName.ChangeState, this.currentState, (int) TurnState.START_POWERUPS);
        }
        else
        {
            this.currentState = flowStates.Find(x => x.state == TurnState.START_POWERUPS);
            this.currentState.Enter(this.currentCharacter);
        }
    }
    
    public void EndTurn()
    {
        GD.Print("Ending turn.");
       this.OnTurnEnd?.Invoke();
    }

    public void EndBattle(bool playerWin)
    {
        this.currentPhaseState = PhaseState.BATTLE_END;

        //GD.Print("Ending Battle!");
    }
}


