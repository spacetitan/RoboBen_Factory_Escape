using Godot;
using System;

[GlobalClass]
public partial class Character : Node
{
    [Signal] public delegate void StatsChangedEventHandler();
    
    public enum TargetType
    {
        NONE,
        SINGLE,
        SELF,
        ALL,
        EVERYONE
    }
    
    public int health = 0;
    public int armor = 0;
    
    public ModifierHandler modifierHandler = null;
    public StatusHandler statusHandler = null;
    
    public override void _Ready()
    {
        InitializeCharacter();
    }
    
    protected virtual void InitializeCharacter()
    {
        this.modifierHandler = new ModifierHandler();
        this.statusHandler = new StatusHandler(this);
    }
    
    protected void SetHealth(int value)
    {
        this.health = value;
        EmitSignal(SignalName.StatsChanged);
    }
    
    public virtual void TakeDamage(int amount, Modifier.Type type = Modifier.Type.DMG_TAKEN)
    {
        if(this.health <= 0 || amount <= 0) { return; }

        int modifiedAmount = this.modifierHandler.GetModifiedValue(amount, type);
        this.health -= modifiedAmount;
        EmitSignal(SignalName.StatsChanged);
    }

    public virtual void Heal(int amount, Modifier.Type type = Modifier.Type.HEALED)
    {
        if(this.health <= 0 || amount <= 0) { return; }

        int modifiedAmount = this.modifierHandler.GetModifiedValue(amount, type);
        this.health += modifiedAmount;
        EmitSignal(SignalName.StatsChanged);

    }
    
    public void SetArmor(int value)
    {
        this.armor = Math.Clamp(value, 0, 999);
        EmitSignal(SignalName.StatsChanged);
    }

    public void AddArmor(int armor)
    {
        if(armor == 0)
        {
            return;
        }

        this.armor += armor;
        EmitSignal(SignalName.StatsChanged);
    }

    public void ResetArmor()
    {
        this.armor = 0;
        EmitSignal(SignalName.StatsChanged);
    }
}
