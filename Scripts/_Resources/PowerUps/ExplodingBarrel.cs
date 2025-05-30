using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class ExplodingBarrel : PowerUp
{
	public override void InitializePowerUp(PowerUpHandler owner)
	{
		base.InitializePowerUp(owner);
	}

	public override void ActivatePowerUp()
	{
		BattleModel model = UIManager.instance.currentModel as BattleModel;
        
		DamageEffect damage = new DamageEffect(this.value, this.playSFX);
		damage.Execute(model.player.GetTargets(Character.TargetType.ALL));
		GD.Print("Boom");
		base.ActivatePowerUp();
	}
}

