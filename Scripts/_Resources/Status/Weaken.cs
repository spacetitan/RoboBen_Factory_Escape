using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class Weaken : Status
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
			modifier = enemy.modifierHandler.GetModifier(Modifier.Type.DMG_DEALT);
			modifierValue = modifier.GetValue("weaken");
		}
		else if(target is Player)
		{
			Player player = (Player)target;
			modifier = player.modifierHandler.GetModifier(Modifier.Type.DMG_DEALT);
			modifierValue = modifier.GetValue("weaken");
		}
		else {GD.Print("Target not applicable"); return; }

		if(modifierValue == null)
		{
			modifierValue = ModifierValue.CreateModifierValue("weaken", ModifierValue.Type.PERCENT);
		}

		if (this.duration >= 8)
		{
			modifierValue.percentVal = -.8f;
		}
		else
		{
			modifierValue.percentVal = this.duration * -.1f;
		}
        
		modifier.AddNewValue(modifierValue);

		if(stacks <= 0 && duration <= 0 && modifier != null)
		{
			modifier.RemoveValue("weaken");
		}
	}
}
