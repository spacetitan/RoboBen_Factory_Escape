using Godot;
using System;

public partial class RunHUDView : UIView
{
    UIDisplay nameDisplay = null;
    UIDisplay textureDisplay = null;
    UIDisplay healthDisplay = null;
    UIDisplay moneyDisplay = null;
    UIDisplay reRollDisplay = null;

    public override void _Ready()
    {
        Panel leftPanel = GetNode<Panel>("%LeftPanel");
        this.nameDisplay = leftPanel.GetNode<UIDisplay>("NameDisplay");
        this.textureDisplay = leftPanel.GetNode<UIDisplay>("TextureDisplay");
        this.healthDisplay = leftPanel.GetNode<UIDisplay>("HealthDisplay");
        this.moneyDisplay = leftPanel.GetNode<UIDisplay>("MoneyDisplay");
        this.reRollDisplay = leftPanel.GetNode<UIDisplay>("ReRollDisplay");
    }

    public override void Enter()
    {
        Run run = RunManager.instance.currentRun;
        this.nameDisplay.SetData(run.playerData.name);
        this.textureDisplay.SetData(null, run.playerData.texture);
        this.healthDisplay.SetData(run.playerData.health.ToString() + " / " + run.playerData.maxHealth, ResourceManager.instance.HUDIcons["health"]);
        this.moneyDisplay.SetData(run.gold.ToString(), ResourceManager.instance.HUDIcons["money"]);
        this.reRollDisplay.SetData(run.reRolls.ToString(), ResourceManager.instance.HUDIcons["reRoll"]);
    }
}
