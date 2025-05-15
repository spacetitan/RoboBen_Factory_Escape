using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class PlayerData : CharacterData
{
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
        instance.maxHealth = instance.health = maxHealth;
        instance.handSize = handSize;

        return instance;
    }

    public Godot.Collections.Dictionary<StringName, Variant> savePlayerData()
    {
        return new Godot.Collections.Dictionary<StringName, Variant>()
        {
            {"id",(int) this.id},
            {"texture", this.texture},
            {"health", this.health},
            {"maxHealth", this.maxHealth},
            {"handSize", this.handSize},
        };
    }
}
