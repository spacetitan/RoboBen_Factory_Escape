using Godot;
using System;
using System.Collections.Generic;

public partial class Modifier : Resource
{
	public enum Type { NONE, DMG_DEALT, DMG_TAKEN, HEALING, HEALED, ARMOR_GEN }

    [Export] public Type type;
	List<ModifierValue> modifierValues = new List<ModifierValue>();

	public Modifier(Type type)
	{
		this.type = type;
	}

    public ModifierValue GetValue(String source)
	{
		foreach (ModifierValue value in modifierValues)
		{
			if(value.source == source)
			{
				return value;
			}
		}

		//GD.Print("No values found.");
		return null;
	}

    public void AddNewValue(ModifierValue value)
	{
		ModifierValue tempValue = GetValue(value.source);

		if(tempValue == null)
		{
			this.modifierValues.Add(value);
		}
		else
		{
			tempValue.flatVal = value.flatVal;
			tempValue.percentVal = value.percentVal;
		}
	}

    public void RemoveValue(String source)
	{
		foreach (ModifierValue value in this.modifierValues)
		{
			if(value.source == source)
			{
				//remove signals and destroy value
			}
		}
	}

    public void ClearValues()
	{
		this.modifierValues.Clear();
	}

    public int GetModifiedValue(int baseVal)
	{
		int flatResult = baseVal;
		float percentResult = 1.0f;

		foreach (ModifierValue value in this.modifierValues)
		{
			switch(value.type)
			{
				case ModifierValue.Type.FLAT:
				flatResult += value.flatVal;
				break;

				case ModifierValue.Type.PERCENT:
				percentResult += value.percentVal;
				break;

				case ModifierValue.Type.NONE:
				default:
				GD.Print("No Modifier value type set.");
				break;
			}
		}

		return (int) Math.Clamp(flatResult * percentResult, 0, 1000);
	}

	public int GetNumberOfModifiedValues()
	{
		int count = 0;

		foreach (ModifierValue value in this.modifierValues)
		{
			count++;
		}

		return count;
	}
}
public partial class ModifierValue : RefCounted
{
	public enum Type {	NONE, PERCENT, FLAT }
	[Export] public Type type { get; private set; } 
	[Export] public float percentVal;
	[Export] public int flatVal;
	[Export] public String source  { get; private set; }

	public static ModifierValue CreateModifierValue(String modifierSource, Type modifierValueType)
	{
		ModifierValue newModifier = new ModifierValue();
		newModifier.source = modifierSource;
		newModifier.type = modifierValueType;
		return newModifier;
	}
}
