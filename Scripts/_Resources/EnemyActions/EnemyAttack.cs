using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class EnemyAttack : EnemyAction
{
	[Export] int numOfAtt = 1;
	[Export] int hpThreshold = 0;
	
	public override void InitializeAction(Enemy enemy)
	{
		base.InitializeAction(enemy);
	}

	public override bool IsPerformable()
	{
		if (this.type == ActionType.CONDITIONAL)
		{
			bool isLow = this.owner.data.health <= hpThreshold;
			return isLow;	
		}
		
		return base.IsPerformable();
	}

	public override void PerformAction(Character target)
	{
		//GD.Print(enemy.data.name + " attacks!");
		Vector2 start = this.owner.GlobalPosition;
		Vector2 end = target.GlobalPosition + new Vector2(0,-target.Size.Y/2);
		DamageEffect damage = new DamageEffect(this.owner.GetModifiedAttack(this.value), this.sound);
		List<Character> targetList = new List<Character>(){target};

		Tween tween = this.owner.CreateTween().SetTrans(Tween.TransitionType.Quint);
		tween.TweenProperty(this.owner, "global_position", end, .4);

		tween.TweenCallback(Callable.From(() => damage.Execute(targetList)));
		if(this.numOfAtt > 1)
		{
			for(int i = 0; i < this.numOfAtt-1; i++)
			{
				tween.TweenInterval(.35);
				tween.TweenCallback(Callable.From(() => damage.Execute(targetList)));
			}
		}

		tween.TweenInterval(.25);
		tween.TweenProperty(this.owner, "global_position", start, .4);

		tween.Finished += () => 
		{
			base.PerformAction(target);
		};
	}
}

