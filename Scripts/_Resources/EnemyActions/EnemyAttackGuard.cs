using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class EnemyAttackGuard : EnemyAction
{
	[Export] int guardValue = 3;
	[Export] int numOfAtt = 1;
	[Export] int hpThreshold = 0;
	
	public override void InitializeAction(Enemy enemy)
	{
		base.InitializeAction(enemy);
	}
	
	public override string GetIntent()
	{
		if (this.numOfAtt > 1)
		{
			return this.owner.GetModifiedAttack(this.value).ToString() + " X " + numOfAtt.ToString();	
		}
		else
		{
			return this.owner.GetModifiedAttack(this.value).ToString();
		}
	}

	public override bool IsPerformable()
	{
		if (this.type == ActionType.CONDITIONAL)
		{
			bool isLow = this.owner.data.health <= hpThreshold;
			return isLow;	
		}
		
		GD.Print("Checking action");
		
		return base.IsPerformable();
	}

	public override void PerformAction(Action callback = null)
	{
		//GD.Print(enemy.data.name + " attacks!");
		Vector2 start = this.owner.GlobalPosition;
		Vector2 end = this.target.GlobalPosition + new Vector2(this.target.Size.X/2, this.target.Size.Y/4);
		DamageEffect damage = new DamageEffect(this.owner.GetModifiedAttack(this.value), this.sound);
		List<Character> targetList = new List<Character>(){this.target};

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
		
		GuardEffect guard = new GuardEffect(value + this.owner.data.defense, this.sound);
		tween.TweenInterval(.35);
		tween.TweenCallback(Callable.From(() => guard.Execute(this.owner)));
		AudioManager.instance.sfxPlayer.Play(ResourceManager.instance.audio[ResourceManager.AudioID.ARMOR]);
		
		tween.TweenInterval(.25);

		tween.Finished += () => 
		{
			callback?.Invoke();
			base.PerformAction();
		};
	}
}

