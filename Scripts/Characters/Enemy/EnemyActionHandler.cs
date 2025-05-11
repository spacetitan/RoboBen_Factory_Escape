using Godot;
using System;
using System.Collections.Generic;

public partial class EnemyActionHandler : Node
{
     [Export] public Enemy owner { get; private set;}
	public Character target { get; private set;}
	public List<EnemyAction> enemyActions{ get; private set; } = new List<EnemyAction>();

	private int totalWeight = 0;

	public EnemyActionHandler(Enemy enemy)
	{
		this.owner = enemy;
		SetActions(this.owner);
		SetTargets(this.owner.GetTree().GetFirstNodeInGroup("Player") as Player);
	}

	private void SetTargets(Character value)
	{
		target = value;

		foreach(EnemyAction enemyAction in this.enemyActions)
		{
			enemyAction.target = target;
		}
	}

	private void SetActions(Enemy enemy)
	{
		//GD.Print("Setting Actions for " + enemy.data.name);
		if(enemy.data.actions == null || enemy.data.actions.Length < 1)
		{
			GD.Print("No actions set.");
			return;
		}

		this.enemyActions.Clear();

		foreach (EnemyAction enemyAction in enemy.data.actions)
		{
			EnemyAction action = enemyAction.CreateInstance();
			action.InitializeAction(enemy);
			this.enemyActions.Add(action);
		}

		SetupChances();
	}

	private void SetupChances()
	{
		this.totalWeight = 0;

		EnemyAction action;
		foreach(EnemyAction enemyAction in this.enemyActions)
		{
			action = enemyAction;

			if((action == null) || action.type != EnemyAction.ActionType.CHANCE)
			{
				continue;
			}

			totalWeight += action.chanceWeight;
			action.accumulatedweight = totalWeight;
		}
	}

	public EnemyAction GetAction()
	{
		EnemyAction action = GetFirstConditionalAction();

		if(action != null)
		{
			return action;
		}

		return GetChanceBasedAction();
	}

	private EnemyAction GetFirstConditionalAction()
	{
		EnemyAction action;

		foreach(EnemyAction enemyAction in this.enemyActions)
		{
			action = enemyAction;

			if((action == null) || action.type != EnemyAction.ActionType.CONDITIONAL)
			{
				continue;
			}

			if(action.IsPerformable())
			{
				return action;
			}
		}
		//GD.Print("No conditional Action, returning null");
		return null;
	}

	private EnemyAction GetChanceBasedAction()
	{
		EnemyAction action;
		RandomNumberGenerator rng = new RandomNumberGenerator();
		rng.Randomize();

		int roll = rng.RandiRange(0, totalWeight-1);
		
		if(roll == totalWeight)
		{
			GD.Print("Out of bounds");
		}

		foreach(EnemyAction enemyAction in this.enemyActions)
		{
			action = enemyAction;

			if((action == null) || action.type != EnemyAction.ActionType.CHANCE)
			{
				continue;
			}
			if(action.accumulatedweight > roll)
			{
				return action;
			}
		}
		GD.Print("No Action, returning null");
		return null;
	}
}
