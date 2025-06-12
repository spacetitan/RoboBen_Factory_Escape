using Godot;
using System.Collections.Generic;
using System.Diagnostics;

public partial class CardUI : Panel
{
    private TextureRect cardTexture = null;
    private RichTextLabel titleLabel = null;
    private RichTextLabel descLabel = null;
    private RichTextLabel costLabel = null;
    private RichTextLabel genLabel = null;
    public Area2D playArea { get; private set; } = null;

    public CardData data { get; private set; } = null;
    public CardStateMachine stateMachine { get; private set; } = null;
    public HandView hand { get; private set; } = null;
    public Player player { get; private set; } = null;
    public List<Character> targets = new List<Character>();
    public Tween tween = null;
    
    public StyleBox cardstyleDefault { get; private set;} = ResourceLoader.Load<StyleBox>("res://Themes/Card/CardPanel.tres");
    public StyleBox cardstyleDrag { get; private set;} = ResourceLoader.Load<StyleBox>("res://Themes/Card/CardPanelDragging.tres");
    public StyleBox cardstyleHover { get; private set;} = ResourceLoader.Load<StyleBox>("res://Themes/Card/CardPanelHover.tres");
    public StyleBox cardstyleDisabled { get; private set;} = ResourceLoader.Load<StyleBox>("res://Themes/Card/CardPanelDisabled.tres");
    public StyleBox cardStyleBurn { get; private set;} = ResourceLoader.Load<StyleBox>("res://Themes/Card/CardPanelBurning.tres");
    public StyleBox cardStylePlay { get; private set;} = ResourceLoader.Load<StyleBox>("res://Themes/Card/CardPanelPlaying.tres");
    
    public bool isPlayable{ get; private set;} = false;
    public bool isDisabled{ get; private set;} = true;
    public bool inBurner { get; private set;} = false;
    
    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        this.cardTexture = this.GetNode<TextureRect>("%CardTexture");
        this.titleLabel = this.GetNode<RichTextLabel>("%TitleLabel");
        this.descLabel = this.GetNode<RichTextLabel>("%DescLabel");
        this.costLabel = this.GetNode<RichTextLabel>("%CostLabel");
        this.genLabel = this.GetNode<RichTextLabel>("%GenLabel");
        this.playArea = this.GetNode<Area2D>("%PlayArea2D");
    }

    public void SetData(CardData data, HandView hand)
    {
        this.data = data;
        
        this.cardTexture.Texture = data.Texture;
        this.titleLabel.Text = data.cardName;
        this.descLabel.Text = data.GetDefaultToolip();
        this.costLabel.Text = "Cost: " + data.cardCost.ToString();
        this.genLabel.Text = "Gen: " + data.cardGen.ToString();
        
        this.stateMachine = new CardStateMachine(this);
        this.hand = hand;
        this.player = this.hand.player;
        this.player.StatsChanged += OnStatsChanged;
    }
    
    public void OnStatsChanged()
    {
        SetPlayable(this.player.CanPlayCard(this));
        this.descLabel.Text = this.data.GetModifiedTooltip(this.player.modifierHandler, null);
    }
    
    public void SetPlayable(bool value)
    {
        this.isPlayable = value;
        if(!isPlayable)
        {
            this.costLabel.AddThemeColorOverride("font_color", Colors.Red);
            this.cardTexture.Modulate = new Color(1,1,1,.05f);
            this.AddThemeStyleboxOverride("panel", this.cardstyleDisabled);
        }
        else
        {
            this.costLabel.RemoveThemeColorOverride("font_color");
            this.cardTexture.Modulate = new Color(1,1,1,1);
            this.AddThemeStyleboxOverride("panel", this.cardstyleDefault);
        }
    }
    public void SetDisabled(bool value)
    {
        this.isDisabled = value;
        SetPlayable(this.player.CanPlayCard(this));
    }

    public void SetBurner(bool val = false)
    {
        this.inBurner = val;
        
        if (val)
        {
            this.AddThemeStyleboxOverride("panel", this.cardStyleBurn);
        }
        else
        {
            this.AddThemeStyleboxOverride("panel", this.cardstyleDrag);
        }
    }

    public void AnimateToPosition(Vector2 newPosition, float duration)
    {
        this.isDisabled = true;
        tween = CreateTween().SetTrans(Tween.TransitionType.Circ).SetEase(Tween.EaseType.Out);
        tween.TweenProperty(this, "position", newPosition, duration);
        tween.Finished += () => { this.isDisabled = false; };
    }

    public void Play()
    {
        if(this.data == null)
        {
            return;
        }
    
        this.player.AddEnergy(-this.data.cardCost);
        data.ApplyEffects(this.targets, this.player.playerData, player.modifierHandler);
        EventManager.instance.EmitSignal(EventManager.SignalName.CardPlayed);
        this.player.PlayCard(this);
    }

    public void Burn()
    {
        if(this.data == null || this.data.cardGen == 0)
        {
            return;
        }

        this.hand.BurnCard(this);
    }

    public void DestroyCard()
    {
        this.player.StatsChanged -= OnStatsChanged;
        ClearTargets();
        QueueFree();
    }
    
    void OnPlayAreaEntered(Area2D area)
    {
        if(area.IsInGroup("Burner"))
        {
            this.inBurner = true;
            this.AddThemeStyleboxOverride("panel", this.cardStyleBurn);
        }
        
        if (this.isPlayable && area.IsInGroup("PlayArea") && data.targetType != Character.TargetType.SINGLE)
        {
            this.AddThemeStyleboxOverride("panel", this.cardStylePlay);

            if (player.GetTargets(this.data.targetType) == null)
            {
                return;
            }

            foreach (Character character in this.player.GetTargets(this.data.targetType))
            {
                character.ShowTargeted(true);
                this.targets.Add(character);   
            }
        }
    }
    void OnPlayAreaExited(Area2D area)
    {
        //GD.Print("Area Exited");
        
        if(area.IsInGroup("Burner"))
        {
            this.inBurner = false;

            if (!this.isDisabled)
            {
                this.AddThemeStyleboxOverride("panel", this.cardstyleDrag);
            }
        }

        if (area.IsInGroup("PlayArea") && data.targetType != Character.TargetType.SINGLE)
        {
            ClearTargets();
        }
    }

    public void ClearTargets()
    {
        foreach (Character character in this.targets)
        {
            character.ShowTargeted(false);
        }
        
        this.targets.Clear();
    }

    public override void _Input(InputEvent inputEvent)
    {
        this.stateMachine.OnInput(inputEvent);
    }
    
    void OnGuiInput(InputEvent inputEvent)
    {
        this.stateMachine.OnGuiInput(inputEvent);
    }

    void OnMouseEntered()
    {
        this.stateMachine.OnMouseEntered();
    }

    void OnMouseExited()
    {
        this.stateMachine.OnMouseExited();
    }
}
