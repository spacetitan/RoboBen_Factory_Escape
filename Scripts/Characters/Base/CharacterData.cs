using Godot;
using System;

public partial class CharacterData : Resource
{
    public enum CharacterID { NONE, ROBOBEN, ROBODEV, GRUBBOID, GRUBBIG, GRUBBFLY, TORTIGRUB, GRUBBMANTIS }
    
    [Export] public CharacterID id { get; protected set; } = CharacterID.NONE;
    [Export] public StringName name { get; protected set; } = "";
    [Export] public string desc { get; protected set; } = "";
    [Export] public Texture2D texture { get; protected set; } = null;
    [Export] public int health { get; protected set; } = 0;
    [Export] public int maxHealth { get; protected set; } = 1;

    public void SetHealth(int health)
    {
        this.health = health;
    }

    public void AddMaxHealth(int health)
    {
        this.maxHealth += health;
        this.health += health;
    }

    public virtual CharacterData CreateInstance()
    {
        CharacterData instance = (CharacterData)this.Duplicate();
        instance.id = id;
        instance.name = name;
        instance.texture = texture;
        instance.maxHealth = instance.health = maxHealth;

        return instance;
    }
}
