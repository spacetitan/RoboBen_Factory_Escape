using Godot;
using System;

public partial class DeckButton : Control
{
    private TextureRect deckTexture = null;
    private RichTextLabel deckLabel = null;
    private Button button = null;
    private CardPile cardPile = null;
    
    [Export] public Texture2D buttonTexture { get; set; } = null;

    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        this.deckTexture = this.GetNode<TextureRect>("%DeckTexture");
        if (this.buttonTexture != null)
        {
            this.deckTexture.Texture = this.buttonTexture;    
        }
        
        this.deckLabel = this.GetNode<Panel>("%AmountPanel").GetNode<RichTextLabel>("%DeckLabel");
        this.button = this.GetNode<Button>("%Button");
        this.button.Pressed += () =>
        {
            if (this.cardPile != null)
            {
                
            }
        };
    }

    public void SetData(CardPile cardPile)
    {
        this.cardPile = cardPile;
        this.deckLabel.Text = this.cardPile.cards.Count.ToString();
    }
}
