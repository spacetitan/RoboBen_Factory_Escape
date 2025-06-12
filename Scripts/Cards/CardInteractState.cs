using Godot;
using System;
using System.Linq;

public partial class CardInteractState : RefCounted
{
    [Signal]
	public delegate void ChangeStateEventHandler(CardInteractState fromState, int toState);
	public CardStateMachine.CardStates state;
	public CardUI cardUI;

	public CardInteractState(CardStateMachine.CardStates initialState)
	{
		this.state = initialState;
	}

	public virtual void Enter(){}

	public virtual void Exit(){}

	public virtual void OnInput(InputEvent inputEvent){}

	public virtual void OnGuiInput(InputEvent inputEvent){}

	public virtual void OnMouseEntered(){}

	public virtual void OnMouseExited(){}
}

public partial class CardDefaultState : CardInteractState
{
	public CardStateMachine.CardStates states = CardStateMachine.CardStates.DEFAULT;

    public CardDefaultState(CardStateMachine.CardStates initialState) : base(initialState)
    {
		this.state = initialState;
    }

    public override void Enter()
	{
		if(cardUI.tween != null && cardUI.tween.IsRunning())
		{
			cardUI.tween.Kill();
		}

		if (cardUI.isPlayable)
		{
			cardUI.AddThemeStyleboxOverride("panel", cardUI.cardstyleDefault);
		}
		else
		{
			cardUI.AddThemeStyleboxOverride("panel", cardUI.cardstyleDisabled);
		}
		
		if (cardUI.hand != null)
		{
			cardUI.hand.UpdateHand();
		}
	}

	public override void OnGuiInput(InputEvent inputEvent)
	{
		if(inputEvent.IsActionPressed("LeftMouse"))
		{
			//GD.Print("Clicked");
			cardUI.PivotOffset = cardUI.GetGlobalMousePosition() - cardUI.GlobalPosition;
			EmitSignal(SignalName.ChangeState, this, (int)CardStateMachine.CardStates.CLICKED);
		}
	}

	public override void OnMouseEntered()
	{
        cardUI.AddThemeStyleboxOverride("panel", cardUI.cardstyleHover);
	}

	public override void OnMouseExited()
	{
		if (cardUI.isPlayable)
		{
			cardUI.AddThemeStyleboxOverride("panel", cardUI.cardstyleDefault);
		}
		else
		{
			cardUI.AddThemeStyleboxOverride("panel", cardUI.cardstyleDisabled);
		}
	}
}

public partial class CardClickedState : CardInteractState
{
	public CardStateMachine.CardStates states = CardStateMachine.CardStates.CLICKED;

    public CardClickedState(CardStateMachine.CardStates initialState) : base(initialState)
    {
		this.state = initialState;
    }

    public override void Enter()
	{
		this.cardUI.playArea.Monitorable = true;
	}

	public override void OnInput(InputEvent inputEvent)
	{
		if(inputEvent is InputEventMouseMotion)
		{
			EmitSignal(SignalName.ChangeState, this, (int)CardStateMachine.CardStates.DRAGGING);
		}
	}
}

public partial class CardDraggingState : CardInteractState
{
	public CardStateMachine.CardStates states = CardStateMachine.CardStates.DRAGGING;
	const float DRAG_MINIMUM_THRESHOLD = .03f;
	bool minimumDragTimeElapsed = false;

    public CardDraggingState(CardStateMachine.CardStates initialState) : base(initialState)
    {
		this.state = initialState;
    }

    public override void Enter()
	{
		cardUI.AddThemeStyleboxOverride("panel", cardUI.cardstyleDrag);

		this.minimumDragTimeElapsed = false;
		SceneTreeTimer thresholdTimer = cardUI.GetTree().CreateTimer(DRAG_MINIMUM_THRESHOLD, false);
		thresholdTimer.Timeout += () => {minimumDragTimeElapsed = true;};
		AudioManager.instance.sfxPlayer.Play(ResourceManager.instance.audio[ResourceManager.AudioID.CARD_GRABBED]);
	}

	public override void Exit()
	{
		
	}

	public override void OnInput(InputEvent inputEvent)
	{
		bool singleTargeted = cardUI.data.isSingleTargeted();
		bool mouseMotion = inputEvent is InputEventMouseMotion;
		bool confirm = inputEvent.IsActionReleased("LeftMouse") || inputEvent.IsActionPressed("LeftMouse");

		if(singleTargeted && mouseMotion && cardUI.isPlayable)
		{
			EmitSignal(SignalName.ChangeState, this, (int)CardStateMachine.CardStates.AIMING);
			return;
		}

		if(mouseMotion)
		{
			this.cardUI.GlobalPosition = cardUI.GetGlobalMousePosition() - cardUI.PivotOffset;
		}
		else if(this.minimumDragTimeElapsed && confirm)
		{
			cardUI.GetViewport().SetInputAsHandled();
			EmitSignal(SignalName.ChangeState, this, (int)CardStateMachine.CardStates.RELEASED);
		}
	}
}

public partial class CardAimingState : CardInteractState
{
	public CardStateMachine.CardStates states = CardStateMachine.CardStates.AIMING;
	private float MOUSE_Y_SNAPBACK_THRESHOLD = 800;

    public CardAimingState(CardStateMachine.CardStates initialState) : base(initialState)
    {
		this.state = initialState;
    }

    public override void Enter()
	{
		cardUI.targets.Clear();

		Vector2 offset = new Vector2(cardUI.hand.Size.X /8, -cardUI.hand.Size.Y + 10);
		offset.X -= cardUI.Size.X / 2;
		cardUI.AnimateToPosition(offset, 0.2f);
		
		MOUSE_Y_SNAPBACK_THRESHOLD = cardUI.GetViewportRect().Size.Y - (cardUI.hand.Size.Y / 2); //refine this value
		
		cardUI.playArea.Monitorable = false;
		EventManager.instance.EmitSignal(EventManager.SignalName.CardAimStarted, cardUI);
	}

	public override void Exit()
	{
		EventManager.instance.EmitSignal(EventManager.SignalName.AimEnded);
	}

	public override void OnInput(InputEvent inputEvent)
	{
		bool mouseMotion = inputEvent is InputEventMouseMotion;
		bool mouseAtBotton = cardUI.GetGlobalMousePosition().Y > (MOUSE_Y_SNAPBACK_THRESHOLD);
		
		//GD.Print(MOUSE_Y_SNAPBACK_THRESHOLD);
		//GD.Print(cardUI.GetGlobalMousePosition().Y);

		if((mouseMotion && mouseAtBotton) || inputEvent.IsActionPressed("RightMouse"))
		{
			EmitSignal(SignalName.ChangeState, this, (int)CardStateMachine.CardStates.DEFAULT);
		}
		else if(inputEvent.IsActionReleased("LeftMouse") || inputEvent.IsActionPressed("LeftMouse"))
		{
			this.cardUI.GetViewport().SetInputAsHandled();
			EmitSignal(SignalName.ChangeState, this, (int)CardStateMachine.CardStates.RELEASED);
		}
	}
}

public partial class CardReleasedState : CardInteractState
{
	public CardStateMachine.CardStates states = CardStateMachine.CardStates.RELEASED;
	bool played = false;

    public CardReleasedState(CardStateMachine.CardStates initialState) : base(initialState)
    {
		this.state = initialState;
    }

    public override void Enter()
	{
		played = false;

		if(cardUI.inBurner)
		{
			//GD.Print("Burning Card");
			cardUI.Burn();
			return;
		}
		//
		if(cardUI.targets.Any() && cardUI.isPlayable)
		{
			played = true;
			cardUI.Play();
			return;
		}
		
		if(!cardUI.isPlayable)
		{
			//GD.Print("Card is not playable");
		}
		else
		{
			GD.Print("No Targets!");
		}
	}

	public override void OnInput(InputEvent inputEvent)
	{
		if(played) {return;}
		EmitSignal(SignalName.ChangeState, this, (int)CardStateMachine.CardStates.DEFAULT);
		//this.cardUI.hand.UpdateHand();
	}
}
