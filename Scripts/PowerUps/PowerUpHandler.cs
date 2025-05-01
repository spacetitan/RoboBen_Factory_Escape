using Godot;
using System;
using System.Collections.Generic;

public partial class PowerUpHandler : Node
{
    public List<PowerUp> powerUps = new List<PowerUp>();

    public Godot.Collections.Dictionary<StringName, Variant> SavePowerUps()
    {
        Godot.Collections.Dictionary<StringName, Variant> data = new Godot.Collections.Dictionary<StringName, Variant>();
        for (int i = 0; i < powerUps.Count; i++)
        {
            data.Add("Power up " + i, powerUps[i].id);
        }
        return data;
    }
}
