using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Slime : EventData
{
	public override void InitializeEvent()
	{
		this.choices.Add(new Choice("Feed it a card?", "The card dissolves in the slimes body. Lets leave."));
	}
}

public partial class Choice : EventChoice
{
	public Choice(string body, string outcome) : base(body, outcome)
	{
		this.body = body;
		this.outcomeText = outcome;
	}

	public override void Outcome()
	{
		UIManager.instance.popUpModel.OpenDeckPopUp(RunManager.instance.currentRun.playerDeck, "Remove a card", true);
	}
}

