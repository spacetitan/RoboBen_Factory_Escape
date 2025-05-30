using Godot;
using System;

public partial class ShopItemPanel : UIView
{
    private CardDisplayUI itemDisplay = null;
    private UIButton buyButton = null;
    
    private PowerUp powerUp = null;
    private CardData cardData = null;
    
    private float cost = 0;
    
    public Action onBuyCallback = null;
    
    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        this.itemDisplay = GetNode<CardDisplayUI>("%ItemDisplayUI");
        this.itemDisplay.GetSceneNodes();
        this.buyButton = GetNode<UIButton>("%BuyButton");
        this.buyButton.button.Pressed += OnClickBuyButton;
    }

    public void OnClickBuyButton()
    {
        RunManager.instance.currentRun.TakeMoney((int)this.cost);
        
        if (this.powerUp != null)
        {
            RunManager.instance.AddPowerUp(this.powerUp);
        }
        else if(this.cardData != null)
        {
            RunManager.instance.AddCard(this.cardData);
        }
        
        RoomHUDView hud = UIManager.instance.hudModel.views[UIModel.ViewID.ROOM_HUD] as RoomHUDView;
        hud.UpdateData();
        
        this.buyButton.SetData("Purchased");
        this.buyButton.button.Disabled = true;
        this.onBuyCallback?.Invoke();
        //darken card display
    }

    public void UpdateData()
    {
        if (RunManager.instance.currentRun.gold < this.cost)
        {
            this.buyButton.button.Disabled = true;
        }
    }

    public void ResetData()
    {
        this.buyButton.button.Disabled = false;
        this.powerUp = null;
        this.cardData = null;
    }

    public void SetData(PowerUp powerUp, Action callback = null)
    {
        this.itemDisplay.SetData(powerUp, null, true);
        this.powerUp = powerUp;
        
        switch (powerUp.rarity)
        {
            case PowerUp.Rarity.COMMON:
                this.cost = 10.0f;
                break;
            
            case PowerUp.Rarity.UNCOMMON:
                this.cost = 20.0f;
                break;
            
            case PowerUp.Rarity.RARE:
                this.cost = 30.0f;
                break;
            
            case PowerUp.Rarity.NONE:
            default:
                GD.Print("No rarity set");
                break;
        }
        
        if (RunManager.instance.currentRun.ContainsPowerUp(PowerUp.PowerUpID.MEMBERSHIP))
        {
            this.cost *= .8f;
            this.cost = Mathf.RoundToInt(this.cost);
        }
        
        this.buyButton.SetData("Cost: " + this.cost);

        if (RunManager.instance.currentRun.gold >= this.cost)
        {
            this.buyButton.button.Disabled = false;
        }
        else
        {
            this.buyButton.button.Disabled = true;
        }

        this.onBuyCallback = callback;
    }
    
    public void SetData(CardData card, Action callback = null)
    {
        this.itemDisplay.SetData(card, null, true);
        this.cardData = card;
        
        switch (card.rarity)
        {
            case CardData.Rarity.COMMON:
                this.cost = 10;
                break;
            
            case CardData.Rarity.UNCOMMON:
                this.cost = 20;
                break;
            
            case CardData.Rarity.RARE:
                this.cost = 30;
                break;
            
            case CardData.Rarity.NONE:
            default:
                GD.Print("No rarity set");
                break;
        }
        
        if (RunManager.instance.currentRun.ContainsPowerUp(PowerUp.PowerUpID.MEMBERSHIP))
        {
            this.cost *= .8f;
            this.cost = Mathf.RoundToInt(this.cost);
        }
        
        this.buyButton.SetData("Cost: " + this.cost);
        
        if (RunManager.instance.currentRun.gold >= this.cost)
        {
            this.buyButton.button.Disabled = false;
        }
        else
        {
            this.buyButton.button.Disabled = true;
        }
        
        this.onBuyCallback = callback;
    }
}
