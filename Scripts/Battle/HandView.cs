using Godot;
using System;
using System.Collections.Generic;

public partial class HandView : UIView
{
    private PackedScene cardScene = ResourceLoader.Load<PackedScene>("res://Scenes/Battle/card_ui.tscn");
    
    [Export] private Curve handCurve = null;
    [Export] private Curve rotationCurve = null;

    [Export] private float maxRotation = 5.0f;
    [Export] private float xSep = -10.0f;
    [Export] private float yMin = 0f;
    [Export] private float yMax = -15.0f;
    
    public Player player { get; private set; }
    public List<CardUI> cards { get; private set; } = new List<CardUI>();
    
    public override void _Ready()
    {
        // EventManager.instance.CardPlayed += OnCardPlayed;
        // EventManager.instance.CardBurned += OnCardBurned;
        // this.ChildOrderChanged += UpdateHand;
    }

    public void SetData(Player player)
    {
        this.player = player;
        this.player.hand = this;
    }

    public override void Exit()
    {
        foreach (CardUI card in this.cards)
        {
            card.DestroyCard();
        }
        this.cards.Clear();
    }

    public void AddCard(CardData cardData)
    {
        CardUI newCardUI = this.cardScene.Instantiate() as CardUI;
        newCardUI.GetSceneNodes();
        newCardUI.SetData(cardData, this);
        float size = this.Size.X / 4;
        newCardUI.SetSize(new Vector2(size, size*1.4f));
        AddChild(newCardUI);
        this.cards.Add(newCardUI);
        AudioManager.instance.sfxPlayer.Play(ResourceManager.instance.audio[ResourceManager.AudioID.DRAW_CARD]);
        UpdateHand();
    }

    public void BurnCard(CardUI card)
    {
        this.player.AddEnergy(card.data.cardGen);
        EventManager.instance.EmitSignal(EventManager.SignalName.CardBurned);
        this.player.DiscardCard(card);
        AudioManager.instance.sfxPlayer.Play(ResourceManager.instance.audio[ResourceManager.AudioID.CARD_BURNED]);
    }

    public void DiscardCard(CardUI card)
    {
        this.cards.Remove(card);
        card.DestroyCard();
    }

    public void OnChildOrderChanged()
    {
        UpdateHand();
    }

    public void UpdateHand()
    {
        if (this.GetChildren().Count > 0)
        {
            CardUI card = GetChild(0) as CardUI;
            int cardCount = GetChildren().Count;
            float AllCardsSize = card.Size.X * cardCount + xSep * (cardCount -1);
            float finalXSep = this.xSep;

            if (AllCardsSize > Size.X)
            {
                finalXSep = (Size.X - card.Size.X * cardCount) / (cardCount - 1);
                AllCardsSize = Size.X;
            }

            float offset = (Size.X - AllCardsSize) / 2;

            for (int i = 0; i < cardCount; i++)
            {
                CardUI cardUI = GetChild(i) as CardUI;
                float yMultiplier = handCurve.Sample(1.0f / (cardCount - 1) * i);
                float rotMultiplier = rotationCurve.Sample(1.0f / (cardCount - 1) * i);

                if (cardCount == 1)
                {
                    yMultiplier = 0;
                    rotMultiplier = 0;
                }

                float finalX = offset + cardUI.Size.X * i + finalXSep * i;
                float finalY = yMin + yMax * yMultiplier;
			
                cardUI.AnimateToPosition(new Vector2(finalX, finalY), .5f);
                cardUI.RotationDegrees = maxRotation * rotMultiplier;
            }
        }
    }

    public void EnableHand()
    {
        foreach (CardUI cardUi in this.GetChildren())
        {
            cardUi.OnStatsChanged();
        }
    }

    public List<CardUI> GetHand()
    {
        List<CardUI> hand = new List<CardUI>();
        
        foreach (CardUI cardUi in this.GetChildren())
        {
            hand.Add(cardUi);
        }
        
        return hand;
    }
}
