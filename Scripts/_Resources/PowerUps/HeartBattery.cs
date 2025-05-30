using Godot;
using System;

[GlobalClass]
public partial class HeartBattery : PowerUp
{
	public override void InitializePowerUp(PowerUpHandler owner)
	{
		EventManager.instance.CardBurned -= ActivatePowerUp;
		EventManager.instance.CardBurned += ActivatePowerUp;
		base.InitializePowerUp(owner);
	}

	public override void ActivatePowerUp()
	{
		BattleModel model = UIManager.instance.currentModel as BattleModel;
		float roll = RunManager.instance.currentRun.rng.RandfRange(0.0f, 10.0f);

		if (roll < 2.0f)
		{
			model.player.Heal(this.value);
		}
		BattleHUDView view = UIManager.instance.hudModel.views[UIModel.ViewID.BATTLE_HUD] as BattleHUDView;
		view.UpdateStats();
		
		base.ActivatePowerUp();
	}
	
	public override void DestroyPowerUp()
	{
		EventManager.instance.CardBurned -= ActivatePowerUp;
	}
}

