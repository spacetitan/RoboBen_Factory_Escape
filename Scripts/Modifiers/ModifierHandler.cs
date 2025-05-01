using Godot;
using System;
using System.Collections.Generic;

public partial class ModifierHandler : RefCounted
{
    public List<Modifier> modifiers { get; private set; } = new List<Modifier>();

    public ModifierHandler()
    {
        this.modifiers.Add(new Modifier(Modifier.Type.DMG_TAKEN));
        this.modifiers.Add(new Modifier(Modifier.Type.DMG_DEALT));
        this.modifiers.Add(new Modifier(Modifier.Type.HEALED));
        this.modifiers.Add(new Modifier(Modifier.Type.HEALING));
    }

    public bool HasModifier(Modifier.Type modifierType)
    {
        foreach (Modifier modifier in modifiers)
        {
            if(modifier.type == modifierType)
            {
                return true;
            }
        }

        return false;
    }

    public Modifier GetModifier(Modifier.Type modifierType)
    {
        foreach (Modifier modifier in modifiers)
        {
            if(modifier.type == modifierType)
            {
                return modifier;
            }
        }

        return null;
    }

    public int GetModifiedValue(int baseVal, Modifier.Type modifierType)
    {
        Modifier modifier = GetModifier(modifierType);

        if(modifier == null)
        {
            return baseVal;
        }

        return modifier.GetModifiedValue(baseVal);
    }

    public String DebugOutput()
    {
        String output = "Active Modifiers: ";

        int count = 0;
        foreach(Modifier modifier in modifiers)
        {
            count += modifier.GetNumberOfModifiedValues();
        }

        output += count.ToString();
        return output;
    }
}
