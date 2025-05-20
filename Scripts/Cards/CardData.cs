using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class CardData : Resource
{
    public enum CardID
    {
        NONE,
        ATTACK,
        GUARD,
        POISONATTACK,
        REPAIRKIT
    }
    public enum Rarity { NONE, COMMON, UNCOMMON, RARE};

    [Export] public CardID id = CardID.NONE;
    [Export] public string cardName { get; private set; } = "";
    [Export(PropertyHint.MultilineText)] public string cardDesc { get; private set; } = "";
    
    [Export] public Texture2D Texture = null;
    [Export] public AudioStream playSFX { get; private set;}
    [Export] public Rarity rarity { get; private set; } = Rarity.NONE;
    [Export] public Character.TargetType targetType { get; private set; } = Character.TargetType.NONE;
    [Export] public int cardValue { get; private set; } = 1;
    [Export] public int cardCost { get; private set; } = 1;
    [Export] public int cardGen { get; private set; } = 1;
    [Export] public bool isExhaust { get; private set; } = false;
    
    public bool isSingleTargeted()
    {
        return this.targetType == Character.TargetType.SINGLE;
    }
    
    public virtual void ApplyEffects(List<Character> targets, PlayerData playerData, ModifierHandler modifiers)
    {
        return;
    }
    
    public bool startWithVowel()
    {
        switch (cardName[0])
        {
            case 'A':
            case 'E':
            case 'I':
            case 'O':
                return true;
        }

        return false;
    }
    
    public virtual string GetDefaultToolip()
    {
        return this.cardDesc.Replace("{value}", this.cardValue.ToString());
    }
    
    public virtual string GetModifiedTooltip(ModifierHandler playerModifiers, ModifierHandler enemyModifiers)
    {
        return this.cardDesc.Replace("{value}", this.cardValue.ToString());
    }

    public virtual CardData CreateInstance()
    {
        CardData instance = (CardData)this.Duplicate();
        instance.id = this.id;
        instance.cardName = this.cardName;
        instance.cardDesc = this.cardDesc;
        instance.Texture = this.Texture;
        instance.playSFX = this.playSFX;
        instance.rarity = this.rarity;
        instance.targetType = this.targetType;
        instance.cardValue = this.cardValue;
        instance.cardCost = this.cardCost;
        instance.cardGen = this.cardGen;
        instance.isExhaust = this.isExhaust;
		
        return instance;
    }
}
