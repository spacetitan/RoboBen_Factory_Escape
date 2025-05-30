using Godot;
using System;

[GlobalClass]
public partial class Battery : PowerUp
{
	public override void InitializePowerUp(PowerUpHandler owner)
	{
		base.InitializePowerUp(owner);
	}

	public override void ActivatePowerUp()
	{
		BattleModel model = UIManager.instance.currentModel as BattleModel;
		model.player.AddEnergy(1);

		BattleHUDView view = UIManager.instance.hudModel.views[UIModel.ViewID.BATTLE_HUD] as BattleHUDView;
		view.UpdateStats();
		base.ActivatePowerUp();
	}
}

