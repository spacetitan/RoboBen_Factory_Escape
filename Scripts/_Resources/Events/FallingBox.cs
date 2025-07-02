using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class FallingBox : EventData
{
	public override void InitializeEvent()
	{
		this.choices.Add(new RunPast("Run past it!", ""));
		this.choices.Add(new Dodge("Dodge outta the way!", ""));
	}
}

public partial class RunPast : EventChoice
{
	public RunPast(string body, string outcome) : base(body, outcome)
	{
		this.body = body;
		this.outcomeText = outcome;
	}

	public override void Outcome()
	{
		RandomNumberGenerator rng = RunManager.instance.currentRun.rng;
		float roll = rng.RandfRange(0.0f, 10.0f);

		if (roll < 6.0f)
		{
		    this.outcomeText = "You ran past the boxes saftely!";
		}
		else
		{
			CardData data = RunManager.instance.GetRandomCardFromDeck();
			RunManager.instance.RemoveCard(data);
			
		    this.outcomeText = "A card fell out of your pocket!\n(You lost a " + data.cardName + " card)";
		}
	}
}

public partial class Dodge : EventChoice
{
	public Dodge(string body, string outcome) : base(body, outcome)
	{
		this.body = body;
		this.outcomeText = outcome;
	}

	public override void Outcome()
	{
		RandomNumberGenerator rng = RunManager.instance.currentRun.rng;
		float roll = rng.RandfRange(0.0f, 10.0f);

		if (roll < 4.0f)
		{
		    this.outcomeText = "You dodged! Time to skedaddle!";
		}
		else
		{
		    this.outcomeText = "The rubble fell on you! (-7 hp)";
		    RunManager.instance.currentRun.TakeHealth(7);
		}
	}
}


