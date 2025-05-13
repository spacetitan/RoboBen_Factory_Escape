using Godot;
using System;

[GlobalClass]
public partial class ReinforcedArmor : PowerUp
{
	public override void InitializePowerUp(PowerUpHandler owner)
	{
		base.InitializePowerUp(owner);
	}

	public override void ActivatePowerUp()
	{
		BattleModel model = UIManager.instance.currentModel as BattleModel;
		GuardEffect armor = new GuardEffect(this.value, this.playSFX);
		armor.Execute(model.player);
		base.ActivatePowerUp();
	}
}

