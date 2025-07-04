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
			RunManager.instance.AddPowerUp(RunManager.instance.GetAvailablePowerUp());
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
		});
	}
}

