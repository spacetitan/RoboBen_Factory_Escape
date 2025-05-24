using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class EnemyGuardAll : EnemyAction
{
	public override void InitializeAction(Enemy enemy)
	{
		base.InitializeAction(enemy);
	}
	
	public override string GetIntent()
	{
		return this.owner.GetModifiedDefence(this.value).ToString();
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
		
		SceneTree tree = this.owner.GetTree();
		List<Character> temp = new List<Character>();
		foreach(Node node in tree.GetNodesInGroup("Enemy"))
		{
			if (node is Character)
			{
				temp.Add(node as Character);	
			}
		}
		
		GuardEffect guard = new GuardEffect(this.owner.GetModifiedDefence(this.value), this.sound);
		guard.Execute(temp);

		this.owner.GetTree().CreateTimer(0.6, false).Timeout += () => 
		{
			callback?.Invoke();
			base.PerformAction();
		};
	}
}

