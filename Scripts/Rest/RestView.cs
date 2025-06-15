using Godot;
using System;

public partial class RestView : UIView
{
    private UIButton restButton = null;
    private UIButton fullHealButton = null;
    private ShopItemPanel itemPanel = null;
    private Control playerSpawn = null;
    private UIButton reRollButton = null;
    private UIButton removeCardButton = null;
    
    public Vector2 playerSize { get; private set; } = Vector2.Zero;
    
    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        this.restButton = this.GetNode<UIButton>("RestButton");
        this.restButton.SetData("Rest\n-Heal 30%-");
        this.restButton.button.Pressed += () =>
        {
            RunManager.instance.Rest();
            this.restButton.button.Disabled = true;
            OnButtonPressed();
        };

        this.fullHealButton = this.GetNode<UIButton>("FullHealButton");
        this.fullHealButton.SetData("Full Heal\nCost:10");
        this.fullHealButton.button.Pressed += () =>
        {
            RunManager.instance.FullHeal();
            this.fullHealButton.button.Disabled = true;
            OnButtonPressed();
        };
        
        this.itemPanel = this.GetNode<ShopItemPanel>("%ShopItemPanel");
        
        this.playerSpawn = this.GetNode<Control>("PlayerSpawn");
        this.playerSize = new Vector2(this.playerSpawn.Size.X, this.playerSpawn.Size.X);
        
        this.reRollButton = this.GetNode<UIButton>("%ReRollButton");
        this.reRollButton.SetData("Buy a Re-Roll\nCost: 10", ResourceManager.instance.HUDIcons[ResourceManager.HUDIconID.REROLL]);
        this.reRollButton.button.Pressed += () =>
        {
            RunManager.instance.currentRun.TakeMoney(10);
            this.reRollButton.button.Disabled = true;
            RunManager.instance.currentRun.BuyReRoll();
            OnButtonPressed();
        };
        
        this.removeCardButton = this.GetNode<UIButton>("%RemoveCardButton");
        this.removeCardButton.SetData("Remove a card\nCost: 10", ResourceManager.instance.HUDIcons[ResourceManager.HUDIconID.CARD]);
        this.removeCardButton.button.Pressed += () =>
        {
            RunManager.instance.currentRun.TakeMoney(10);
            UIManager.instance.popUpModel.OpenDeckPopUp(RunManager.instance.currentRun.playerDeck, "Remove a card", true);
            this.removeCardButton.button.Disabled = true;
            OnButtonPressed();
        };
    }

    public void SetData()
    {
        Player player = ResourceManager.instance.playerScene.Instantiate() as Player;
        player.GetSceneNodes();
        this.playerSpawn.AddChild(player);
        player.SetPlayerData(RunManager.instance.currentRun);
        player.SetCustomMinimumSize(this.playerSize);
        
        this.itemPanel.SetData(ResourceManager.instance.cards[CardData.CardID.REPAIRKIT]);

        this.restButton.button.Disabled = false;
        if (RunManager.instance.currentRun.gold > 10)
        {
            this.fullHealButton.button.Disabled = false;
            this.reRollButton.button.Disabled = false;
            this.removeCardButton.button.Disabled = false;
        }
        else
        {
            this.fullHealButton.button.Disabled = true;
            this.reRollButton.button.Disabled = true;
            this.removeCardButton.button.Disabled = true;
        }
    }

    public override void Enter()
    {
        
    }

    private void OnButtonPressed()
    {
        RoomHUDView hud = UIManager.instance.hudModel.views[UIModel.ViewID.ROOM_HUD] as RoomHUDView;
        hud.UpdateData();
        
        this.itemPanel.UpdateData();
        if (RunManager.instance.currentRun.gold < 10)
        {
            this.fullHealButton.button.Disabled = true;
            this.reRollButton.button.Disabled = true;
            this.removeCardButton.button.Disabled = true;
        }
    }

    public override void Exit()
    {
        if (this.playerSpawn.GetChildren().Count > 0)
        {
            Player player = this.playerSpawn.GetChild(0) as Player;
            player.DestroyPlayer();
        }

        
    }
}
