using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class HealthShrine : EventData
{
	public override void InitializeEvent()
	{
		this.choices.Add(new GambleHealth("This is button text", "this adds to the body"));
	}
}

public partial class GambleHealth : EventChoice
{
	public GambleHealth(string body, string outcome) : base(body, outcome)
	{
		this.body = body;
		this.outcomeText = outcome;
	}

	private float chance = 4.0f;
	private float costPercent = .75f;
	private float cost = 0.0f;

	public override void Enter()
	{
		SetCost(3);
	}

	private void SetCost(int cost)
	{
		this.cost = cost;

		if (this.cost < 1)
		{
			this.cost = 1;
		}

		if (RunManager.instance.currentRun.playerData.health < cost)
		{
			this.isDisabled = true;
		}
		else
		{
			this.isDisabled = false;
		}

		this.body = "Make an offering?\n" + cost + "Hp";
	}

	public override void Outcome()
	{
		RunManager.instance.currentRun.TakeHealth((int)this.cost);

		RandomNumberGenerator rng = new RandomNumberGenerator();
		float roll = rng.RandfRange(0.0f, 10.0f);

		if (roll <= chance)
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

			RunManager.instance.AddPowerUp(powerUp);
			chance *= .5f;
		}
		else
		{
			this.outcomeText = "Nothing happens. Try again?";
		}

		int cost = (int)(this.cost * (this.costPercent+1));
		GD.Print(cost);
		SetCost(cost);
	}
}

