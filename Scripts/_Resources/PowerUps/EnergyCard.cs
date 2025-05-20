using Godot;
using System;

[GlobalClass]
public partial class EnergyCard : PowerUp
{
	int num = 0;
	
	public override void InitializePowerUp(PowerUpHandler owner)
	{
		EventManager.instance.CardBurned -= ActivatePowerUp;
		EventManager.instance.CardBurned += ActivatePowerUp;
		
		base.InitializePowerUp(owner);
	}

	public override void ActivatePowerUp()
	{
		BattleModel model = UIManager.instance.currentModel as BattleModel;
		
		num++;
		if (value >= 2)
		{
			model.player.AddEnergy(this.value);
            
			num = 0;
		}
		
		base.ActivatePowerUp();
	}
}

