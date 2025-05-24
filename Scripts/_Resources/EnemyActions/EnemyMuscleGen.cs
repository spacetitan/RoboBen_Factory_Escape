using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class EnemyMuscleGen : EnemyAction
{
	public override void InitializeAction(Enemy enemy)
	{
		base.InitializeAction(enemy);
	}
	
	public override string GetIntent()
	{
		return this.value.ToString();
	}

	public override bool IsPerformable()
	{
		return base.IsPerformable();
	}

	public override void PerformAction(Action callback = null)
	{
		if(this.owner == null || this.target == null)
		{
			if(this.owner == null)
			{
				GD.Print("Enemy is null");
			}
			else if(target == null)
			{
				GD.Print("target is null");
			}
			else
			{
				GD.Print("this ability has already been used");
			}
			return;
		}

		if(this.type == ActionType.CONDITIONAL)
		{
			isUsed = true;
		}

		Muscle muscle = ResourceManager.instance.statuses[Status.StatusID.MUSCLE].Duplicate() as Muscle;
		muscle.SetDuration(this.value);
		StatusEffect status = new StatusEffect(muscle, this.sound);
		status.Execute(new List<Character>(){this.owner});

		this.owner.GetTree().CreateTimer(0.6, false).Timeout += () => 
		{
			callback?.Invoke();
			base.PerformAction();
		};
	}
}

