using Godot;
using System;
using System.Collections.Generic;

public partial class DeckPopUpView : UIView
{
    private RichTextLabel titleLabel = null;
    private ScrollContainer scrollContainer = null;
    private GridContainer cardContainer = null;
    private Button closeButton = null;
    
    private List<CardDisplayUI> cards = new List<CardDisplayUI>();
    
    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        this.titleLabel = this.GetNode<Panel>("%TitlePanel").GetNode<RichTextLabel>("%TitleLabel");
        this.scrollContainer = this.GetNode<ScrollContainer>("%CardScrollContainer");
        this.cardContainer = this.scrollContainer.GetNode<GridContainer>("%CardContainer");
        this.closeButton = this.GetNode<Button>("%CloseButton");
        this.closeButton.Pressed += () =>
        {
            UIManager.instance.popUpModel.ClosePopup("deck");
        };
    }

    public void OpenPopUp(CardPile data, string title)
    {
        this.titleLabel.Text = title;
        this.cards = new List<CardDisplayUI>();

        foreach (CardData card in data.cards)
        {
            CardDisplayUI display = ResourceManager.instance.displayCard.Instantiate() as CardDisplayUI;
            display.GetSceneNodes();
            display.SetData(card);
            display.SetCustomMinimumSize(new Vector2(this.cardContainer.Size.X/8, (this.cardContainer.Size.X/8) * 1.4f));
            this.cards.Add(display);
            this.cardContainer.AddChild(display);
        }
        
        ShowView();
    }

    public override void Exit()
    {
        foreach (CardDisplayUI card in cards)
        {
            card.QueueFree();
        }
    }
}
