using Godot;
using System;

[GlobalClass]
public partial class EnemyData : CharacterData
{
    [Export] public EnemyAction[] actions { get; private set; }
    [Export] public int attack { get; private set; } = 0;
    [Export] public int defense { get; private set; } = 0;
    
    public override EnemyData CreateInstance()
    {
        EnemyData instance = (EnemyData)this.Duplicate();
        instance.id = id;
        instance.name = name;
        instance.texture = texture;
        instance.maxHealth = instance.health = maxHealth;

        instance.actions = new EnemyAction[this.actions.Length];
        for (int i = 0; i < this.actions.Length; i++)
        {
            instance.actions[i] = this.actions[i].CreateInstance();
        }

        instance.attack = this.attack;
        instance.defense = this.defense;
        
        return instance;
    }
}
