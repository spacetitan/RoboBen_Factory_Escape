using Godot;
using System;
using System.Collections.Generic;

public partial class DeckPopUpView : UIView
{
    private RichTextLabel titleLabel = null;
    private ScrollContainer scrollContainer = null;
    private GridContainer cardContainer = null;
    private UIButton closeButton = null;
    
    private List<CardDisplayUI> cards = new List<CardDisplayUI>();
    private Vector2 cardSize = new Vector2();
    
    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        this.titleLabel = this.GetNode<Panel>("%TitlePanel").GetNode<RichTextLabel>("%TitleLabel");
        this.scrollContainer = this.GetNode<ScrollContainer>("%CardScrollContainer");
        this.cardContainer = this.scrollContainer.GetNode<GridContainer>("%CardContainer");
        this.closeButton = this.GetNode<UIButton>("%CloseButton");
        this.closeButton.SetData("Close");
        this.closeButton.button.Pressed += () =>
        {
            UIManager.instance.popUpModel.ClosePopup(UIModel.ViewID.DECK);
        };
        
        this.cardSize = new Vector2(this.scrollContainer.Size.X/7, (this.scrollContainer.Size.X/7) * 1.4f);
    }

    public void OpenPopUp(CardPile data, string title, bool removeCard = false)
    {
        this.titleLabel.Text = title;
        this.cards = new List<CardDisplayUI>();

        foreach (CardData card in data.cards)
        {
            CardDisplayUI display = ResourceManager.instance.displayCard.Instantiate() as CardDisplayUI;
            display.GetSceneNodes();

            if (removeCard)
            {
                display.SetData(card, () =>
                {
                    RunManager.instance.RemoveCard(card);
                    UIManager.instance.popUpModel.ClosePopup(UIModel.ViewID.DECK);
                    
                    RoomHUDView hud = UIManager.instance.hudModel.views[UIModel.ViewID.ROOM_HUD] as RoomHUDView;
                    hud.UpdateData();
                });
            }
            else
            {
                display.SetData(card, null, true);
            }

            
            display.SetCustomMinimumSize(this.cardSize);
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
