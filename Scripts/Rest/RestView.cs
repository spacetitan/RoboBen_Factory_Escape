using Godot;
using System;

public partial class RestView : UIView
{
    private UIButton restButton = null;
    private UIButton fullHealButton = null;
    private ShopItemPanel itemPanel = null;
    private Control playerSpawn = null;
    
    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        this.restButton = this.GetNode<UIButton>("RestButton");
        this.restButton.SetData("Rest - 30% heal");
        this.restButton.button.Pressed += () =>
        {
            RunManager.instance.Rest();
            this.restButton.button.Disabled = true;
            RoomHUDView hud = UIManager.instance.hudModel.views["roomHUD"] as RoomHUDView;
            hud.UpdateHealthUI();
        };

        this.fullHealButton = this.GetNode<UIButton>("FullHealButton");
        this.fullHealButton.SetData("Full Heal - 10 money");
        this.fullHealButton.button.Pressed += () =>
        {
            RunManager.instance.FullHeal();
            this.fullHealButton.button.Disabled = true;
            RoomHUDView hud = UIManager.instance.hudModel.views["roomHUD"] as RoomHUDView;
            hud.UpdateHealthUI();
        };
        
        this.itemPanel = this.GetNode<ShopItemPanel>("%ShopItemPanel");
        
        this.playerSpawn = this.GetNode<Control>("PlayerSpawn");
    }

    public override void Enter()
    {
        Player player = ResourceManager.instance.playerScene.Instantiate() as Player;
        player.GetSceneNodes();
        this.playerSpawn.AddChild(player);
        player.SetPlayerData(RunManager.instance.currentRun);
        
        this.itemPanel.SetData(ResourceManager.instance.cards[CardData.CardID.REPAIRKIT]);

        this.restButton.button.Disabled = false;
        if (RunManager.instance.currentRun.gold > 10)
        {
            this.fullHealButton.button.Disabled = false;
        }
        else
        {
            this.fullHealButton.button.Disabled = true;
        }
    }

    public void Exit()
    {
        if (this.playerSpawn.GetChildren().Count > 0)
        {
            Player player = this.playerSpawn.GetChild(0) as Player;
            player.DestroyPlayer();
        }

        
    }
}
