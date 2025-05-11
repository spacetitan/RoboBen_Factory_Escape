using Godot;
using System;

public partial class CardDisplayUI : Control
{
    private TextureRect textureRect = null;
    private RichTextLabel titleLabel = null;
    private RichTextLabel descLabel = null;
    private RichTextLabel costLabel = null;
    private RichTextLabel genLabel = null;
    private RichTextLabel typeLabel = null;
    private Button cardButton = null;
    
    private Action onClick = null;
    
    public override void _Ready()
    {
        GetSceneNodes();
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
        // this.cardButton.Pressed += () =>
        // {
        //     if (this.onClick != null)
        //     {
        //         this.onClick.Invoke();
        //     }
        // };
    }

    public void SetData(CardData data, Action onClick = null)
    {
        this.textureRect.Texture = data.Texture;
        this.titleLabel.Text = data.cardName;
        this.descLabel.Text = data.cardDesc;
        this.costLabel.Text = data.cardCost.ToString();
        this.genLabel.Text = data.cardGen.ToString();
        this.typeLabel.Text = "Card";

        if (onClick != null)
        {
            this.onClick = onClick;
        }
    }
    
    public void SetData(PowerUp data, Action onClick = null)
    {
        this.textureRect.Texture = data.texture;
        this.titleLabel.Text = data.name;
        this.descLabel.Text = data.description;
        this.costLabel.Text = "";
        this.genLabel.Text = "";
        this.typeLabel.Text = "Power-Up";

        if (onClick != null)
        {
            this.onClick = onClick;
        }
    }
}
