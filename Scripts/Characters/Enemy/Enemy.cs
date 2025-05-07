using Godot;
using System;

public partial class Enemy : Character
{
    public EnemyData data { get; private set; } = null;
    
    private TextureRect texture = null;
    
    public void GetSceneNodes()
    {
        this.texture = this.GetNode<TextureRect>("%EnemyTexture");
    }

    public void SetData(EnemyData enemyData)
    {
        this.data = enemyData;
        this.texture.Texture = this.data.texture;
    }

    public override void TakeDamage(int amount, Modifier.Type type = Modifier.Type.DMG_TAKEN)
    {
        if(this.health <= 0 || amount <= 0) { return; }
        
        int modifiedAmount = this.modifierHandler.GetModifiedValue(amount, type);
        
        int initial_dmg = modifiedAmount;
        modifiedAmount = Mathf.Clamp(modifiedAmount - armor, 0, modifiedAmount);
        this.armor = Mathf.Clamp(armor - initial_dmg, 0, armor);
        
        this.health = Math.Clamp(this.health - modifiedAmount, 0, this.data.maxHealth);
        EmitSignal(SignalName.StatsChanged);
    }

    public void TakeDirectDamage(int amount, Modifier.Type type = Modifier.Type.DMG_TAKEN)
    {
        this.health = Math.Clamp(this.health - amount, 0, this.data.maxHealth);
        EmitSignal(SignalName.StatsChanged);
    }

    public override void Heal(int amount, Modifier.Type type = Modifier.Type.HEALED)
    {
        if(this.health <= 0 || amount <= 0) { return; }

        int modifiedAmount = this.modifierHandler.GetModifiedValue(amount, type);
        this.health = Math.Clamp(this.health + modifiedAmount, 0, this.data.maxHealth);
        EmitSignal(SignalName.StatsChanged);
    }

    public int GetModifiedAttack(int value)
    {
        return this.modifierHandler.GetModifiedValue(value + this.data.attack, Modifier.Type.DMG_DEALT);
    }

    public int GetModifiedDefence(int value)
    {
        return this.modifierHandler.GetModifiedValue(value + this.data.defense, Modifier.Type.ARMOR_GEN);
    }
}
