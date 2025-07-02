using Godot;
using System;

[GlobalClass]
public partial class ChemMixture : PowerUp
{
	[Export] public int damage = 10;
	public override void InitializePowerUp(PowerUpHandler owner)
	{
		base.InitializePowerUp(owner);
		EventManager.instance.StatusAdded += ActivatePowerUp;
		
	}

	public override void ActivatePowerUp()
	{
		this.value++;

		if (this.value >= 3)
		{
			BattleModel model = UIManager.instance.currentModel as BattleModel;
        
			DamageEffect damage = new DamageEffect(this.damage, this.playSFX);
			damage.Execute(model.player.GetTargets(Character.TargetType.ALL));
			this.value = 0;
		}

		base.ActivatePowerUp();
	}
	
	public override void DestroyPowerUp()
	{
		EventManager.instance.StatusAdded -= ActivatePowerUp;
	}
}

