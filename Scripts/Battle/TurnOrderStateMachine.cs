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
        this.currentPhaseState = PhaseState.BATTLE_START;

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
            //GD.Print("Current character is Player.");
        }
        else if(character is Enemy)
        {
            this.currentPhaseState = PhaseState.ENEMY;
            this.currentCharacter = character;
            //GD.Print("Current character is Enemy.");
        }
        else
        {
            GD.Print("Current character is invalid.");
        }
		
        if (this.currentState != null)
        {
            //GD.Print("current state is not null.");
            this.currentState.EmitSignal(TurnOrderState.SignalName.ChangeState, this.currentState, (int) TurnState.START_POWERUPS);
        }
        else
        {
            RunManager.instance.currentRun.powerUpHandler.ActivatePowerUpsByType(PowerUp.ActivateType.START_OF_COMBAT, () => 
            {
                //GD.Print("current state is null.");
                this.currentState = flowStates.Find(x => x.state == TurnState.START_POWERUPS);
                this.currentState.Enter(this.currentCharacter);
            });
        }
    }
    
    public void EndTurn()
    {  
       this.OnTurnEnd?.Invoke();
    }

    public void EndBattle(bool playerWin, BattleData data)
    {
        this.currentPhaseState = PhaseState.BATTLE_END;
        this.currentCharacter = null;
        this.currentState = null;
        this.OnTurnEnd = null;

        if (playerWin)
        {
            RunManager.instance.currentRun.powerUpHandler.ActivatePowerUpsByType(PowerUp.ActivateType.END_OF_COMBAT, () => 
            {
                if (data.tier == 2)
                {
                    GameOverModel model = UIManager.instance.models[UIManager.UIState.GAMEOVER] as GameOverModel;
                    model.SetData(true);
                    UIManager.instance.ChangeStateTo(UIManager.UIState.GAMEOVER);
                }
                else
                {
                    float money = data.money;
                    if (RunManager.instance.currentRun.ContainsPowerUp(PowerUp.PowerUpID.CROWN))
                    {
                        money *= 1.2f;
                        money = Mathf.RoundToInt(money);
                    }

                    AudioManager.instance.sfxPlayer.Play(ResourceManager.instance.audio[ResourceManager.AudioID.BATTLE_WIN], true);
                    UIManager.instance.popUpModel.OpenBattleWinView((int) money);
                }
            });
        }
        else
        {
            GameOverModel model = UIManager.instance.models[UIManager.UIState.GAMEOVER] as GameOverModel;
            model.SetData(false);
            UIManager.instance.ChangeStateTo(UIManager.UIState.GAMEOVER);
        }

        

        //GD.Print("Ending Battle!");
    }
}


