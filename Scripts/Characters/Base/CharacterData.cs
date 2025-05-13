using Godot;
using System;

public partial class CharacterData : Resource
{
    [Export] public StringName id = "";
    [Export] public StringName name = "";
    [Export] public Texture2D texture = null;
    [Export] public int health = 0;
    [Export] public int maxHealth = 1;

    public void SetHealth(int health)
    {
        this.health = health;
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
