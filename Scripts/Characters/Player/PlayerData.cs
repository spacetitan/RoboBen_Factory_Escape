using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

[GlobalClass]
public partial class PlayerData : CharacterData
{
    [Export] public int handSize = 1;
    
    [Export] public Ability ability { get; private set; }
    [Export] public PowerUp starterPowerUp { get; private set; }
    [Export] public CardPile startingDeck{ get; private set; }

    public void LoadData(Dictionary data)
    {
        this.maxHealth = (int)data["Max Health"];
        this.health = (int)data["Health"];
        this.handSize = (int)data["Hand Size"];
    }

    public override PlayerData CreateInstance()
    {
        PlayerData instance = (PlayerData)this.Duplicate();
        instance.id = id;
        instance.name = name;
        instance.texture = texture;
        instance.maxHealth = instance.health = maxHealth;
        instance.handSize = handSize;

        return instance;
    }

    public Godot.Collections.Dictionary<StringName, Variant> savePlayerData()
    {
        return new Godot.Collections.Dictionary<StringName, Variant>()
        {
            {"ID",(int) this.id},
            {"Health", this.health},
            {"Max Health", this.maxHealth},
            {"Hand Size", this.handSize},
        };
    }
}
