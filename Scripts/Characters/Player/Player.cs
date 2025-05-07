using Godot;
using System;
using System.Collections.Generic;

public partial class Player : Character
{
    public PlayerData playerData = null;
    
    public TextureRect playerTexture = null;
    
    public int energy = 0;

    public void GetSceneNodes()
    {
        this.playerTexture = this.GetNode<TextureRect>("PlayerTexture");
    }

    public void SetPlayerData(PlayerData playerData)
    {
        this.playerData = playerData;
        this.playerTexture.Texture = this.playerData.texture;
        this.energy = 0;
    }

    public override void TakeDamage(int amount, Modifier.Type type = Modifier.Type.DMG_TAKEN)
    {
        if(this.health <= 0 || amount <= 0) { return; }
        
        int modifiedAmount = this.modifierHandler.GetModifiedValue(amount, type);
        
        int initial_dmg = modifiedAmount;
        modifiedAmount = Mathf.Clamp(modifiedAmount - armor, 0, modifiedAmount);
        this.armor = Mathf.Clamp(armor - initial_dmg, 0, armor);
        
        this.health = Math.Clamp(this.health - modifiedAmount, 0, this.playerData.maxHealth);
        EmitSignal(SignalName.StatsChanged);
    }

    public void TakeDirectDamage(int amount, Modifier.Type type = Modifier.Type.DMG_TAKEN)
    {
        this.health = Math.Clamp(this.health - amount, 0, this.playerData.maxHealth);
        EmitSignal(SignalName.StatsChanged);
    }

    public override void Heal(int amount, Modifier.Type type = Modifier.Type.HEALED)
    {
        if(this.health <= 0 || amount <= 0) { return; }

        int modifiedAmount = this.modifierHandler.GetModifiedValue(amount, type);
        this.health = Math.Clamp(this.health + modifiedAmount, 0, this.playerData.maxHealth);
        EmitSignal(SignalName.StatsChanged);
    }
    
    private void SetEnergy(int value)
    {
        this.energy = value;
        EmitSignal(SignalName.StatsChanged);
    }

    public void AddEnergy(int value)
    {
        this.energy += value;
        EmitSignal(SignalName.StatsChanged);
    }

    public void ResetEnergy()
    {
        this.energy = 0;
        EmitSignal(SignalName.StatsChanged);
    }
    
    public bool CanPlayCard(CardUI card)
    {
        return this.energy >= card.data.cardCost;
    }

    public void DrawCard()
    {
        
    }
    
    public void DrawCards(int amount)
    {
        
    }

    public List<Character> GetTargets(TargetType targetType)
    {
        SceneTree tree = this.GetTree();
        List<Character> temp = new List<Character>();

        switch(targetType)
        {
            case TargetType.SELF:
                foreach(Node node in tree.GetNodesInGroup("Player"))
                {
                    temp.Add(node as Character);
                }
                break;
            

            case TargetType.ALL:
                foreach(Node node in tree.GetNodesInGroup("Enemies"))
                {
                    temp.Add(node as Character);
                }
                break;

            case TargetType.EVERYONE:
                foreach(Node node in tree.GetNodesInGroup("Player") + tree.GetNodesInGroup("Enemies"))
                {
                    temp.Add(node as Character);
                }
                break;

            case TargetType.SINGLE:
            case TargetType.NONE:
            default:
                return null;
        }

        return temp;
    }
}
