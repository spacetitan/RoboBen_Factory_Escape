using Godot;
using System;

public partial class CardDisplayUI : Control
{
    private StyleBox defaultStyleBox = ResourceLoader.Load<StyleBox>("res://Themes/Card/CardPanel.tres");
    private StyleBox hoverStyleBox = ResourceLoader.Load<StyleBox>("res://Themes/Card/CardPanelUIHover.tres");
    public StyleBox cardstyleDisabled { get; private set;} = ResourceLoader.Load<StyleBox>("res://Themes/Card/CardPanelDisabled.tres");
    
    private TextureRect textureRect = null;
    private RichTextLabel titleLabel = null;
    private RichTextLabel descLabel = null;
    private RichTextLabel costLabel = null;
    private RichTextLabel genLabel = null;
    private RichTextLabel typeLabel = null;
    private Button cardButton = null;
    
    private Panel costPanel = null;
    private Panel genPanel = null;
    
    private Action onClick = null;
    
    public CardData cardData = null;
    public PowerUp powerUp = null;
    
    public bool glowDisabled = false;
    
    public override void _Ready()
    {
        
    }

    public void GetSceneNodes()
    {
        this.textureRect = this.GetNode<TextureRect>("%CardTexture");
        this.titleLabel = this.GetNode<RichTextLabel>("%TitleLabel");
        this.descLabel = this.GetNode<RichTextLabel>("%DescLabel");
        this.costLabel = this.GetNode<RichTextLabel>("%CostLabel");
        this.genLabel = this.GetNode<RichTextLabel>("%GenLabel");
        this.typeLabel = this.GetNode<RichTextLabel>("%TypeLabel");
        this.cardButton = this.GetNode<Button>("%CardButton");
        this.cardButton.Pressed += () =>
        {
            this.onClick?.Invoke();
        };
        this.AddThemeStyleboxOverride("panel", defaultStyleBox);
        
        this.costPanel = this.GetNode<Panel>("%CostPanel");
        this.genPanel = this.GetNode<Panel>("%GenPanel");
    }

    public void SetData(CardData data, Action onClick = null, bool glowDisabled = false, bool showType = true)
    {
        this.textureRect.Texture = data.Texture;
        this.titleLabel.Text = data.cardName;
        this.descLabel.Text = data.GetDefaultToolip();
        this.costLabel.Text = "Cost: " + data.cardCost.ToString();
        this.genLabel.Text = "Gen: " + data.cardGen.ToString();

        if (showType)
        {
            this.typeLabel.Text = "Card";
        }
        else
        {
            this.typeLabel.Text = "";
        }

        

        if (onClick != null)
        {
            this.onClick = onClick;
        }
        
        this.cardData = data;
        this.glowDisabled = glowDisabled;
    }
    
    public void SetData(PowerUp data, Action onClick = null, bool glowDisabled = false)
    {
        this.textureRect.Texture = data.texture;
        this.titleLabel.Text = data.name;
        this.descLabel.Text = data.description;
        this.costLabel.Text = "";
        this.genLabel.Text = "";
        this.typeLabel.Text = "Power-Up";
        
        this.costPanel.Hide();
        this.genPanel.Hide();

        if (onClick != null)
        {
            this.onClick = onClick;
        }

        this.powerUp = data;
        this.glowDisabled = glowDisabled;
    }

    public void OnMouseEntered()
    {
        if (!this.glowDisabled)
        {
            this.AddThemeStyleboxOverride("panel", hoverStyleBox);   
        }
    }

    public void OnMouseExited()
    {
        if (!this.glowDisabled)
        {
            this.AddThemeStyleboxOverride("panel", defaultStyleBox);   
        }
    }

    public void OnClick()
    {
        AudioManager.instance.sfxPlayer.Play(ResourceManager.instance.audio[ResourceManager.AudioID.BUTTON]);
    }
    
    public void DisableCard()
    {
        this.costLabel.Text = "Cost: " + "[color=#99001e]" + this.cardData.cardCost.ToString() + "[/color]";
        this.textureRect.Modulate = new Color(1,1,1,.05f);
        this.AddThemeStyleboxOverride("panel", this.cardstyleDisabled);
    }
}
