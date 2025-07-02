using Godot;
using System;

[GlobalClass]
public partial class MuscleCard : PowerUp
{
	[Export] public int stacks = 2;

	public override void InitializePowerUp(PowerUpHandler owner)
	{
		EventManager.instance.CardPlayed -= ActivatePowerUp;
		EventManager.instance.CardPlayed += ActivatePowerUp;
		
		base.InitializePowerUp(owner);
	}

	public override void ActivatePowerUp()
	{
		BattleModel model = UIManager.instance.currentModel as BattleModel;

		this.value++;
		if (this.value >= 3)
		{
			Muscle muscle = ResourceManager.instance.statuses[Status.StatusID.MUSCLE].CreateInstance() as Muscle;
			muscle.SetStacks(this.stacks);

			StatusEffect statusEffect = new StatusEffect(muscle, null);
			statusEffect.sender = model.player;
			statusEffect.Execute(model.player);

			this.value = 0;
		}
		base.ActivatePowerUp();
	}
	
	public override void DestroyPowerUp()
	{
		EventManager.instance.CardPlayed -= ActivatePowerUp;
	}
}

