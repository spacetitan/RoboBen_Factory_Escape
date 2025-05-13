using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class Tough : Status
{
	Node target;
	public override void InitializeStatus(Character target)
	{
		this.target = target;
		StatusChanged += OnStatusChanged;
		OnStatusChanged();
		base.InitializeStatus(target);
	}

	public override void ApplyStatus(Character target)
	{
		
		base.ApplyStatus(target);
	}
	
	void OnStatusChanged()
	{
		Modifier modifier;
		ModifierValue modifierValue = null;

		if(target is Enemy)
		{
			Enemy enemy = (Enemy)target;
			modifier = enemy.modifierHandler.GetModifier(Modifier.Type.ARMOR_GEN);
			modifierValue = modifier.GetValue("tough");
		}
		else if(target is Player)
		{
			Player player = (Player)target;
			modifier = player.modifierHandler.GetModifier(Modifier.Type.ARMOR_GEN);
			modifierValue = modifier.GetValue("tough");
		}
		else {GD.Print("Target not applicable"); return; }

		if(modifierValue == null)
		{
			modifierValue = ModifierValue.CreateModifierValue("tough", ModifierValue.Type.PERCENT);
		}

		modifierValue.percentVal = 0.2f;
		modifier.AddNewValue(modifierValue);

		if(stacks <= 0 && duration <= 0 && modifier != null)
		{
			modifier.RemoveValue("tough");
		}
	}
}
