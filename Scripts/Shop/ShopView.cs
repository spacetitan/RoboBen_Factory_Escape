using Godot;
using System;
using System.Collections.Generic;

public partial class ShopView : UIView
{
    private HBoxContainer cardContainer = null;
    private HBoxContainer powerUpContainer = null;
    private Control playerSpawn = null;
    private UIButton reRollButton = null;
    
    private List<ShopItemPanel> itemPanels = new List<ShopItemPanel>();
    
    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        this.playerSpawn = this.GetNode<Control>("%PlayerSpawn");
        this.reRollButton = this.GetNode<UIButton>("ReRollButton");
        this.reRollButton.button.Pressed += ReRoll;
        this.cardContainer = this.GetNode<HBoxContainer>("%CardContainer");
        this.powerUpContainer = this.GetNode<HBoxContainer>("%PowerUpContainer");

        foreach (Node child in this.cardContainer.GetChildren())
        {
            this.itemPanels.Add(child as ShopItemPanel);
        }
        
        foreach (Node child in this.powerUpContainer.GetChildren())
        {
            this.itemPanels.Add(child as ShopItemPanel);
        }
    }

    public void SetData()
    {
        this.reRollButton.SetData("Re-Roll (" + RunManager.instance.currentRun.reRolls.ToString() + ")", ResourceManager.instance.HUDIcons[ResourceManager.HUDIconID.REROLL]);
        
        Player player = ResourceManager.instance.playerScene.Instantiate() as Player;
        player.GetSceneNodes();
        this.playerSpawn.AddChild(player);
        player.SetPlayerData(RunManager.instance.currentRun);
        
        List<PowerUp> powerUps = RunManager.instance.GetAvailablePowerUps(2);
        
        int count = 0;
        foreach (ShopItemPanel panel in itemPanels)
        {
            if (count < 3)
            {
                panel.SetData(RunManager.instance.GetAvailableCard(), () =>
                {
                    OnItemBought(panel);
                });
            }
            else
            {
                panel.SetData(powerUps[count-3], () =>
                {
                    OnItemBought(panel);
                });
            }
        
            count++;
        }
    }

    public override void Enter()
    {
        
    }
    
    public override void Exit()
    {
        if (this.playerSpawn.GetChildren().Count > 0)
        {
            Player player = this.playerSpawn.GetChild(0) as Player;
            player.DestroyPlayer();
        }
    }

    public void OnItemBought(ShopItemPanel item)
    {
        foreach (ShopItemPanel itemPanel in this.itemPanels)
        {
            if (itemPanel != item)
            {
                itemPanel.UpdateData();
            }
        }
    }

    public void ReRoll()
    {
        RunManager.instance.currentRun.SpendReRoll();
        List<PowerUp> powerUps = RunManager.instance.GetAvailablePowerUps(2);
        
        int count = 0;
        foreach (ShopItemPanel panel in itemPanels)
        {
            panel.ResetData();
            
            if (count < 3)
            {
                panel.SetData(RunManager.instance.GetAvailableCard(), () =>
                {
                    OnItemBought(panel);
                });
            }
            else
            {
                panel.SetData(powerUps[count-3], () =>
                {
                    OnItemBought(panel);
                });
            }
        
            count++;
        }
        
        this.reRollButton.SetData("Re-Roll (" + RunManager.instance.currentRun.reRolls + ")", ResourceManager.instance.HUDIcons[ResourceManager.HUDIconID.REROLL]);
    }
}
