using Godot;
using System;

[GlobalClass]
public partial class EnergyCard : PowerUp
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
		
		this.value++;
		if (this.value >= 2)
		{
			model.player.AddEnergy(1);
			AudioManager.instance.sfxPlayer.Play(this.playSFX);
            
			this.value = 0;
		}
		
		base.ActivatePowerUp();
	}

	public override void DestroyPowerUp()
	{
		EventManager.instance.CardBurned -= ActivatePowerUp;
	}
}

