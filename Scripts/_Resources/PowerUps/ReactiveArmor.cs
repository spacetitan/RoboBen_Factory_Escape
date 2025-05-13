using Godot;
using System;

[GlobalClass]
public partial class ReactiveArmor : PowerUp
{
	public override void InitializePowerUp(PowerUpHandler owner)
	{
		EventManager.instance.PlayerDamaged -= ActivatePowerUp;
		EventManager.instance.PlayerDamaged += ActivatePowerUp;
		
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

