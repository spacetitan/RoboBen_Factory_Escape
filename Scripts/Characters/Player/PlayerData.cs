using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class PlayerData : Resource
{
    [Export] public StringName id = "";
    [Export] public StringName name = "";
    [Export] public Texture2D texture = null;
    [Export] public int maxHealth = 1;
    [Export] public int handSize = 1;
    
    [Export] public Ability ability { get; private set; }
    [Export] public PowerUp starterPowerUp { get; private set; }
    [Export] public CardPile startingDeck{ get; private set; }
    
    public PlayerData CreateInstance()
    {
        PlayerData instance = (PlayerData)this.Duplicate();
        instance.id = id;
        instance.name = name;
        instance.texture = texture;
        instance.maxHealth = maxHealth;
        instance.handSize = handSize;

        return instance;
    }

    public Godot.Collections.Dictionary<StringName, Variant> savePlayerData()
    {
        return new Godot.Collections.Dictionary<StringName, Variant>()
        {
            {"id", id},
            {"texture", texture},
            {"maxHealth", maxHealth},
            {"handSize", handSize},
        };
    }
}
