using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Godot.Collections;

[GlobalClass]
public partial class CardPile : Resource
{
    [Signal] public delegate void CardPileSizeChangedEventHandler();
    [Export] private CardData[] cardData;
    
    public List<CardData> cards { get; private set; } = new List<CardData>();
    
    private Random random = new Random();

    public bool isEmpty()
    {
        return cards == null || cards.Count < 1;
    }
    
    public CardData DrawCard()
    {
        if(this.cards == null || !this.cards.Any())
        {
            GD.Print("no cards left.");
            return null;
        }

        CardData drawnCard = cards[0];

        this.cards.RemoveAt(0);

        EmitSignal(SignalName.CardPileSizeChanged);
        return drawnCard;
    }
    
    public void AddCard(CardData card)
    {
        this.cards.Add(card);
        EmitSignal(SignalName.CardPileSizeChanged);
    }

    public void RemoveCard(CardData card)
    {
        this.cards.Remove(card);
        EmitSignal(SignalName.CardPileSizeChanged);
    }

    public void RemoveRandomCard()
    {
        CardData card = this.cards.OrderBy(x => this.random.Next()).First();
        this.cards.Remove(card);
        EmitSignal(SignalName.CardPileSizeChanged);
    }

    public void Shuffle()
    {
        this.cards = this.cards.OrderBy(x => this.random.Next()).ToList();
    }

    public void Clear()
    {
        this.cards.Clear();
        EmitSignal(SignalName.CardPileSizeChanged);
    }
    
    public void LoadDataToDeck()
    {
        this.cards.Clear();
        foreach (CardData data in this.cardData)
        {
            this.cards.Add(data);
        }
    }

    public void LoadDeck(Dictionary data)
    {
        foreach (KeyValuePair<Variant, Variant> cardID in data)
        {
            int ID = (int)cardID.Value;
            AddCard(ResourceManager.instance.cards[(CardData.CardID) ID].CreateInstance());
        }
    }

    public void SetDeck(CardPile cardPile)
    {
        foreach (CardData card in cardPile.cards)
        {
            this.cards.Add(card);
        }
    }

    public Godot.Collections.Dictionary<StringName, Variant> saveCardPile()
    {
        Godot.Collections.Dictionary<StringName, Variant> data = new Godot.Collections.Dictionary<StringName, Variant>();

        for (int i = 0; i < this.cards.Count; i++)
        {
            data.Add("Card " + i.ToString(),(int) cards[i].id);   
        }
        
        return data;
    }
}
