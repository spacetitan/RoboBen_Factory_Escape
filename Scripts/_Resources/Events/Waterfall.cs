using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Waterfall : EventData
{
	public override void InitializeEvent()
	{
		this.choices.Add(new CheckBehindWaterfall("There's something behind it!", ""));
		this.choices.Add(new SitUnderWater("Sit under the water!", "You've become tougher! (+3 max health)\nLets get outta here."));
	}
}

public partial class SitUnderWater : EventChoice
{
	public SitUnderWater(string body, string outcome) : base(body, outcome)
	{
		this.body = body;
		this.outcomeText = outcome;
	}

	public override void Outcome()
	{
		RunManager.instance.currentRun.AddMaxHealth(3);
		RoomHUDView roomHUD = UIManager.instance.hudModel.views[UIModel.ViewID.ROOM_HUD] as RoomHUDView;
		roomHUD.UpdateData();
	}
}

public partial class CheckBehindWaterfall : EventChoice
{
	public CheckBehindWaterfall(string body, string outcome) : base(body, outcome)
	{
		this.body = body;
		this.outcomeText = outcome;
	}

	public override void Outcome()
	{
		int rand = RunManager.instance.currentRun.rng.RandiRange(0, 9);
		if (rand < 3)
		{
			PowerUp powerUp = RunManager.instance.GetAvailablePowerUp();

			if (powerUp.startWithVowel())
			{
				this.outcomeText = "You found an " + powerUp.name + "!";
			}
			else
			{
				this.outcomeText = "You found a " + powerUp.name + "!";
			}

			this.outcomeText += "\nLets get outta here.";
			RunManager.instance.AddPowerUp(powerUp);
		}
		else
		{
			CardData cardStats = RunManager.instance.GetAvailableCard();
            
			if (cardStats.startWithVowel())
			{
				this.outcomeText = "You found an " + cardStats.cardName + " card!";
			}
			else
			{
				this.outcomeText = "You found a " + cardStats.cardName + " card!";
			}
			this.outcomeText += "\nLets get outta here.";
			RunManager.instance.AddCard(cardStats);
			RoomHUDView roomHUD = UIManager.instance.hudModel.views[UIModel.ViewID.ROOM_HUD] as RoomHUDView;
			roomHUD.UpdateData();
		}
	}
}

