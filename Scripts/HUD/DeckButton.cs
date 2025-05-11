using Godot;
using System;

public partial class DeckButton : Control
{
    private TextureRect deckTexture = null;
    private RichTextLabel deckLabel = null;
    private Button button = null;
    private CardPile cardPile = new CardPile();
    
    private string popUpTitle = null;
    
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
                UIManager.instance.popUpModel.OpenDeckPopUp(this.cardPile, this.popUpTitle);
            }
        };
    }

    public void SetData(CardPile cardPile, string popUpTitle = "Deck")
    {
        this.cardPile = new CardPile();
        this.cardPile.SetDeck(cardPile);
        this.deckLabel.Text = this.cardPile.cards.Count.ToString();
        this.popUpTitle = popUpTitle;
    }
}
