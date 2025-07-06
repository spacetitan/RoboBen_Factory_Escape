using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class KingSlime : EventData
{
	public override void InitializeEvent()
	{
		this.choices.Add(new TradePowerUp("Trade for a Power-Up", ""));
		this.choices.Add(new TradeReroll("Trade for Rerolls", ""));
	}
}

public partial class TradePowerUp : EventChoice
{
	public TradePowerUp(string body, string outcome) : base(body, outcome)
	{
		this.body = body;
		this.outcomeText = outcome;
	}

	public override void Outcome()
	{
		UIManager.instance.popUpModel.OpenDeckPopUp(RunManager.instance.currentRun.playerDeck, "Remove a card", true, () =>
		{
			PowerUp powerUp = RunManager.instance.GetAvailablePowerUp();
			
			if (powerUp.startWithVowel())
			{
				this.outcomeText = "You've obtained an " + powerUp.name;
			}
			else
			{
				this.outcomeText = "You've obtained a " + powerUp.name;
			}

			this.outcomeText += "\n\"Thanks!\" he gurgles and then slimes away.";
			
			EventModel model = UIManager.instance.models[UIManager.UIState.EVENT] as EventModel;
			model.AddBodyLabel(this.outcomeText);
			
			RunManager.instance.AddPowerUp(powerUp);
			
			RoomHUDView hud = UIManager.instance.hudModel.views[UIModel.ViewID.ROOM_HUD] as RoomHUDView;
			hud.UpdateData();
		});
	}
}

public partial class TradeReroll : EventChoice
{
	public TradeReroll(string body, string outcome) : base(body, outcome)
	{
		this.body = body;
		this.outcomeText = outcome;
	}

	public override void Outcome()
	{
		UIManager.instance.popUpModel.OpenDeckPopUp(RunManager.instance.currentRun.playerDeck, "Remove a card", true, () =>
		{
			RunManager.instance.currentRun.BuyReRoll(3);
			this.outcomeText = "You got 3 re-rolls!\n\"Thanks!\" he gurgles and then slimes away.";
			
			EventModel model = UIManager.instance.models[UIManager.UIState.EVENT] as EventModel;
			model.AddBodyLabel(this.outcomeText);
			
			RoomHUDView hud = UIManager.instance.hudModel.views[UIModel.ViewID.ROOM_HUD] as RoomHUDView;
			hud.UpdateData();
		});
	}
}

