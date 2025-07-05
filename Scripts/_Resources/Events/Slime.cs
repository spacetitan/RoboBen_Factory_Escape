using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Slime : EventData
{
	public override void InitializeEvent()
	{
		this.choices.Add(new FeedCard("Feed it a card?", ""));
	}
}

public partial class FeedCard : EventChoice
{
	public FeedCard(string body, string outcome) : base(body, outcome)
	{
		this.body = body;
		this.outcomeText = outcome;
	}

	public override void Outcome()
	{
		UIManager.instance.popUpModel.OpenDeckPopUp(RunManager.instance.currentRun.playerDeck, "Remove a card", true,
			() =>
			{
				this.outcomeText = "The card dissolves in the slimes body.\n";
				RandomNumberGenerator rng = new RandomNumberGenerator();
				float roll = rng.RandfRange(0.0f, 10.0f);

				if (roll >= 7.5f)
				{
					this.outcomeText += "Something popped outta the slime!\n";
					PowerUp powerUp = RunManager.instance.GetAvailablePowerUp();
					if (powerUp.startWithVowel())
					{
						this.outcomeText += "You've obtained an " + powerUp.name;
					}
					else
					{
						this.outcomeText += "You've obtained a " + powerUp.name;
					}
				}
				else if (roll < 7.5f && roll >= 3.0f)
				{
					RunManager.instance.currentRun.BuyReRoll(3);
					this.outcomeText += "Something popped outta the slime!\nYou got 3 re-rolls!";
			
					RoomHUDView hud = UIManager.instance.hudModel.views[UIModel.ViewID.ROOM_HUD] as RoomHUDView;
					hud.UpdateData();
				}

				this.outcomeText += " Let's leave.";
				
				EventModel model = UIManager.instance.models[UIManager.UIState.EVENT] as EventModel;
				model.AddBodyLabel(this.outcomeText);
			});
	}
}

