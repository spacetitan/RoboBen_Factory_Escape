using Godot;
using System;

[GlobalClass]
public partial class Status : Resource
{
    public enum TriggerType { NONE, EVENT, START_OF_TURN, END_OF_TURN};
    public enum StackType { NONE, INTENSITY, DURATION};
    public enum StatusID
    {
        NONE,
        MUSCLE,
        POISON,
        REGEN,
        STUN,
        TOUGH,
        WEAKEN,
        INVIS,
    }

    [Signal] public delegate void StatusAppliedEventHandler(Status status);
    [Signal] public delegate void StatusChangedEventHandler();

    [ExportGroup("Status Data")]
    [Export] public StatusID id { get; protected set; } 
    [Export] public StringName name { get; protected set; }
    [Export] public TriggerType triggerType { get; protected set; }
    [Export] public StackType stackType { get; protected set; }
    [Export] public bool canExpire { get; protected set; }
    [Export] public int duration { get; protected set; }
    [Export] public int stacks { get; protected set; }

    [ExportGroup("Status External")]
    [Export] public Texture2D statusIcon { get; protected set; }
    [Export(PropertyHint.MultilineText)] public String desc { get; protected set; }
    [Export] public AudioStream sfx { get; protected set; }

    public int amount { get; protected set; } = 0;

    public virtual void InitializeStatus(Character target) { return; }
    public virtual void ApplyStatus(Character target) 
    {
        GD.Print("Apply: " + id);
        if(this.sfx != null) { AudioManager.instance.sfxPlayer.Play(this.sfx); }
        EmitSignal(SignalName.StatusApplied, this);
        EmitSignal(SignalName.StatusChanged);
    }
    public void SetDuration(int newVal)
    {
        this.duration = newVal;
        EmitSignal(SignalName.StatusChanged);
    }
    public void AddDuration(int newVal)
    {
        this.duration += newVal;
        EmitSignal(SignalName.StatusChanged);
    }
    public void SetStacks(int newVal)
    {
        this.stacks = newVal;
        EmitSignal(SignalName.StatusChanged);
    }
    public void AddStacks(int newVal)
    {
        this.stacks += newVal;
        EmitSignal(SignalName.StatusChanged);
    }

    public virtual Status CreateInstance()
    {
        Status status = (Status) this.Duplicate();
        status.id = this.id;
        status.name = this.name;
        status.triggerType = this.triggerType;
        status.stackType = this.stackType;
        status.canExpire = this.canExpire;
        status.duration = this.duration;
        status.stacks = this.stacks;
        status.sfx = this.sfx;
        status.statusIcon = this.statusIcon;
        status.desc = this.desc;
        status.amount = this.amount;

        return status;
    }
}

