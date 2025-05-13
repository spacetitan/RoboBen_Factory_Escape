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
		model.player.AddEnergy(this.value);
		base.ActivatePowerUp();
	}
}

