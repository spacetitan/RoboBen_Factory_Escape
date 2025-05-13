using Godot;
using System;

[GlobalClass]
public partial class EnergyCard : PowerUp
{
	int value = 0;
	
	public override void InitializePowerUp(PowerUpHandler owner)
	{
		EventManager.instance.CardBurned -= ActivatePowerUp;
		EventManager.instance.CardBurned += ActivatePowerUp;
		
		base.InitializePowerUp(owner);
	}

	public override void ActivatePowerUp()
	{
		BattleModel model = UIManager.instance.currentModel as BattleModel;
		
		value++;
		if (value >= 2)
		{
			model.player.AddEnergy(this.value);
            
			value = 0;
		}
		
		base.ActivatePowerUp();
	}
}

