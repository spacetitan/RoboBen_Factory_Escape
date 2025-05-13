using Godot;
using System;

[GlobalClass]
public partial class MuscleCard : PowerUp
{
	int num = 0;
	public override void InitializePowerUp(PowerUpHandler owner)
	{
		EventManager.instance.CardPlayed -= ActivatePowerUp;
		EventManager.instance.CardPlayed += ActivatePowerUp;
		
		base.InitializePowerUp(owner);
	}

	public override void ActivatePowerUp()
	{
		BattleModel model = UIManager.instance.currentModel as BattleModel;

		num++;
		if (value >= 3)
		{
			Muscle muscle = ResourceManager.instance.statuses["muscle"].Duplicate() as Muscle;
			muscle.SetDuration(this.value);

			StatusEffect statusEffect = new StatusEffect(muscle, null);
			statusEffect.Execute(model.player);
            
			num = 0;
		}
		base.ActivatePowerUp();
	}
}

