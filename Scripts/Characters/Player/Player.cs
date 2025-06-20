using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class Player : Character
{
    public PlayerData playerData = null;
    
    public CardPile deck = new CardPile();
    public CardPile discard = new CardPile();
    
    public TextureRect playerTexture = null;
    public TextureRect targetTexture = null;
    
    public TextureRect armorTexture = null;
    public RichTextLabel armorLabel = null;
    
    public int energy = 0;

    public HandView hand = null;
    const float HAND_DRAW_INTERVAL = .25F;
    const float HAND_DISCARD_INTERVAL = .15F;
    
    private Action OnTurnEnd = null;

    public void GetSceneNodes()
    {
        this.playerTexture = this.GetNode<TextureRect>("PlayerTexture");
        this.targetTexture = this.GetNode<TextureRect>("%TargetTexture");
        this.targetTexture.Hide();
        
        this.armorTexture = this.GetNode<TextureRect>("%ArmorTexture");
        this.armorLabel = this.armorTexture.GetNode<RichTextLabel>("%ArmorLabel");

        this.StatsChanged += UpdateUI;
    }

    public void ConnectEventSignals()
    {
        EventManager.instance.PlayerTurnStarted += ResetAbility;
    }
    
    public void DisconnectEventSignals()
    {
        EventManager.instance.PlayerTurnStarted -= ResetAbility;
    }

    public void SetPlayerData(Run run)
    {
        this.playerData = run.playerData;
        this.playerTexture.Texture = this.playerData.texture;
        this.energy = 0;
        this.playerData.ability.Reset();
        
        this.deck.SetDeck(run.playerDeck);
        this.deck.Shuffle();
    }

    public void DestroyPlayer()
    {
        this.StatsChanged -= UpdateUI;
        DisconnectEventSignals();
        this.QueueFree();
    }

    public override void TakeDamage(int amount, Modifier.Type type = Modifier.Type.DMG_TAKEN)
    {
        if(this.playerData.health <= 0 || amount <= 0) { return; }
        
        int modifiedAmount = this.modifierHandler.GetModifiedValue(amount, type);
        int initial_dmg = modifiedAmount;
        
        Tween tween = CreateTween();
        tween.TweenCallback(Callable.From(()=>{UIManager.instance.vfxModel.Shake(this, 16, .15f);})); // must be less than interval time
        tween.TweenCallback(Callable.From(() =>
        {
            this.playerTexture.Material = ResourceManager.instance.shaders["damage"];
            modifiedAmount = Mathf.Clamp(modifiedAmount - armor, 0, modifiedAmount);
            this.armor = Mathf.Clamp(armor - initial_dmg, 0, armor);

            if (modifiedAmount > 0)
            {
                EventManager.instance.EmitSignal(EventManager.SignalName.PlayerDamaged);
            }
            
            this.playerData.SetHealth(Math.Clamp(this.playerData.health - modifiedAmount, 0, this.playerData.maxHealth));
            EmitSignal(SignalName.StatsChanged);
        }));
        tween.TweenInterval(0.17f); // this is the interval time
        tween.Finished += ()=>
        {
            this.playerTexture.Material = null;

            if(playerData.health <=0)
            {
                EventManager.instance.EmitSignal(EventManager.SignalName.PlayerDied);
            }
        };
    }

    public void TakeDirectDamage(int amount, Modifier.Type type = Modifier.Type.DMG_TAKEN)
    {
        if(this.playerData.health <= 0 || amount <= 0) { return; }
        
        Tween tween = CreateTween();
        tween.TweenCallback(Callable.From(()=>{UIManager.instance.vfxModel.Shake(this, 16, .15f);})); // must be less than interval time
        tween.TweenCallback(Callable.From(() =>
        {
            this.playerTexture.Material = ResourceManager.instance.shaders["damage"];
            UIManager.instance.vfxModel.OnPlayerHit();
            EventManager.instance.EmitSignal(EventManager.SignalName.PlayerDamaged);
            this.playerData.SetHealth(Math.Clamp(this.playerData.health - amount, 0, this.playerData.maxHealth));
            EmitSignal(SignalName.StatsChanged);
        }));
        tween.TweenInterval(0.17f); // this is the interval time
        tween.Finished += ()=>
        {
            this.playerTexture.Material = null;

            if(playerData.health <=0)
            {
                EventManager.instance.EmitSignal(EventManager.SignalName.PlayerDied);
            }
        };
    }

    public override void Heal(int amount, Modifier.Type type = Modifier.Type.HEALED)
    {
        Tween tween = CreateTween();
        tween.TweenCallback(Callable.From(()=>{ UIManager.instance.vfxModel.Shake(this, 16, .15f); })); // must be less than interval time
        tween.TweenCallback(Callable.From(() =>
        {
            this.playerTexture.Material = ResourceManager.instance.shaders["heal"];
            int modifiedAmount = this.modifierHandler.GetModifiedValue(amount, type);
            this.playerData.SetHealth(Math.Clamp(this.playerData.health + modifiedAmount, 0, this.playerData.maxHealth));
            EmitSignal(SignalName.StatsChanged);
            EventManager.instance.EmitSignal(EventManager.SignalName.PlayerHealed);
        }));
        tween.TweenInterval(0.17f); // this is the interval time
        tween.Finished += ()=>
        {
            this.playerTexture.Material = null;
        };
    }

    public void StartTurn(Action onTurnEnd = null)
    {
        this.OnTurnEnd = onTurnEnd;
        DrawCards(this.playerData.handSize, () =>
        {
            BattleHUDView view = UIManager.instance.hudModel.views[UIModel.ViewID.BATTLE_HUD] as BattleHUDView;
            view.ToggleEndTurnButton(false);
        });
    }

    public void EndTurn()
    {
        DiscardCards(this.hand.GetHand(), () =>
        {
            this.OnTurnEnd?.Invoke();
            this.OnTurnEnd = null;
        });
    }

    public void StartOfTurnReset()
    {
        ResetArmor();
        ResetEnergy();
        ResetAbility();
        EventManager.instance.EmitSignal(EventManager.SignalName.PlayerTurnStarted);
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
    
    
    public void ResetAbility()
    {
        if (this.playerData.ability.abilityUsed)
        {
            this.playerData.ability.cooldownTimer++;

            if (this.playerData.ability.cooldownTimer >= this.playerData.ability.cooldown)
            {
                this.playerData.ability.Reset();
            }
        }
    }

    public void UpdateUI()
    {
        // if (this.armor > 0)
        // {
        //     this.armorLabel.Text = this.armor.ToString();
        //     this.armorTexture.Show();
        // }
        // else
        // {
        //     this.armorTexture.Hide();
        // }
        
        BattleHUDView view = UIManager.instance.hudModel.views[UIModel.ViewID.BATTLE_HUD] as BattleHUDView;
        view.UpdateStats();
    }

    public override void ShowTargeted(bool show)
    {
        if (show)
        {
            this.targetTexture.Show();
        }
        else
        {
            this.targetTexture.Hide();
        }
    }

    public bool CanPlayCard(CardUI card)
    {
        return this.energy >= card.data.cardCost;
    }
    
    public void DrawCard()
    {
        ReshuffleDeck();
        this.hand.AddCard(this.deck.DrawCard());
        this.EmitSignal(SignalName.StatsChanged);
    }
    
    public void DrawCards(int amount = 1, Action callback = null)
    {
        Tween tween = CreateTween();

        for(int i = 0; i < amount; i++)
        {
            tween.TweenCallback(Callable.From(DrawCard));
            tween.TweenInterval(HAND_DRAW_INTERVAL);
        }

        tween.Finished += () => 
        {
            this.hand.EnableHand();
            callback?.Invoke();
        };
    }

    public void PlayCard(CardUI card)
    {
        ReshuffleDeck();
        this.hand.DiscardCard(card);
        if (!card.data.isExhaust)
        {
            this.discard.AddCard(card.data);
        }
        else
        {
            GD.Print("Destroying " + card.data.cardName);
        }
        
        this.EmitSignal(SignalName.StatsChanged);	
    }

    public void DiscardCard(CardUI card)
    {
        ReshuffleDeck();
        this.hand.DiscardCard(card);
        this.discard.AddCard(card.data);

        this.EmitSignal(SignalName.StatsChanged);	
    }

    public void DiscardCards(List<CardUI> cards, Action callback = null)
    {
        if (cards.Count > 0)
        {
            Tween tween = CreateTween();

            foreach (CardUI card in cards)
            {
                tween.TweenCallback(Callable.From(() =>
                {
                    DiscardCard(card);
                    AudioManager.instance.sfxPlayer.Play(ResourceManager.instance.audio[ResourceManager.AudioID.CARD_DISCARDED]);
                }));
                tween.TweenInterval(HAND_DRAW_INTERVAL);
            }

            tween.Finished += () => 
            {
                callback?.Invoke();
            };
        }
        else
        {
            callback?.Invoke();
        }
    }

    public void ReshuffleDeck()
    {
        if(this.deck.isEmpty() == false)
        {
            return;
        }

        while(!this.discard.isEmpty())
        {
            this.deck.AddCard(this.discard.DrawCard());
            this.EmitSignal(SignalName.StatsChanged);
        }

        this.deck.Shuffle();
        this.discard.Clear();
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
                foreach(Node node in tree.GetNodesInGroup("Enemy"))
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
