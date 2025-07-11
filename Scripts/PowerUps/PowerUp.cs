using Godot;
using System;

public partial class PowerUp : Resource
{
    [Signal] public delegate void PowerUpActivatedEventHandler();
    public enum PowerUpID
    {
        NONE,
        BATTERY,
        ECARD,
        EBARREL,
        CROWN,
        HBATTERY,
        MEMBERSHIP,
        MCARD,
        REACTARMOR,
        REINFARMOR,
        REPAIRTOOL,
        SECONDWIND,
        LUCKYPENNY,
        CHEMMIXTURE,
    }
    public enum Rarity { NONE, COMMON, UNCOMMON, RARE}
    public enum ActivateType { NONE, START_OF_COMBAT, END_OF_COMBAT, START_OF_TURN, END_OF_TURN, EVENT}
    [Export] public PowerUpID id { get; private set; } = PowerUpID.NONE;
    [Export] public String name { get; private set; } = "";
    [Export(PropertyHint.MultilineText)] public String description { get; private set; } = "";
    [Export] public Texture2D texture { get; private set; } = null;
    [Export] public AudioStream playSFX { get; private set;}
    [Export] public Rarity rarity { get; private set; } = Rarity.NONE;
    [Export] public ActivateType activateType { get; private set; } = ActivateType.NONE;
    [Export] public bool showValue = false;
    [Export] public int value { get; protected set; } = 0;
    protected PowerUpHandler owner = null;

    public virtual void InitializePowerUp(PowerUpHandler owner)
    {
        this.owner = owner;
    }
    
    public virtual void ActivatePowerUp()
    {
        EmitSignal(SignalName.PowerUpActivated);
        RunManager.instance.currentRun.powerUpHandler.UpdateUI();
        //GD.Print("Activating power up: " + this.name);
    }

    public virtual void DestroyPowerUp()
    {
        
    }

    public bool startWithVowel()
    {
        switch (name[0])
        {
            case 'A':
            case 'E':
            case 'I':
            case 'O':
                return true;
        }

        return false;
    }

    public void ResetPowerUp()
    {
        if (this.showValue)
        {
            this.value = 0;   
        }
    }

    public virtual PowerUp CreateInstance()
    {
        PowerUp instance = (PowerUp)this.Duplicate();
        instance.id = this.id;
        instance.name = this.name;
        instance.description = this.description;
        instance.texture = this.texture;
        instance.playSFX = this.playSFX;
        instance.rarity = this.rarity;
        instance.activateType = this.activateType;
        instance.showValue = this.showValue;
        instance.value = this.value;
        
        return instance;
    }
}
