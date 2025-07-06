using Godot;
using System;

[GlobalClass]
public partial class RepairTool : PowerUp
{
	public override void InitializePowerUp(PowerUpHandler owner)
	{
		base.InitializePowerUp(owner);
	}

	public override void ActivatePowerUp()
	{
		BattleModel model = UIManager.instance.currentModel as BattleModel;
		if (model.player != null)
		{
			HealEffect heal = new HealEffect(this.value, this.playSFX);
			heal.Execute(model.player);   
			AudioManager.instance.sfxPlayer.Play(this.playSFX);
		}
		base.ActivatePowerUp();
	}
}

