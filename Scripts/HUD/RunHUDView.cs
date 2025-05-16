using Godot;
using System;
using System.Net.Mime;

public partial class RunHUDView : UIView
{
    private UIDisplay nameDisplay = null;
    private UIDisplay textureDisplay = null;
    private UIDisplay healthDisplay = null;
    private UIDisplay moneyDisplay = null;
    private UIDisplay reRollDisplay = null;
    private DeckButton deckButton = null;

    private GridContainer powerUpContainer = null;
    private UIButton optionsButton = null;
    private UIButton quitButton = null;
    
    public override void _Ready()
    {
        Panel leftPanel = GetNode<Panel>("%LeftPanel");
        this.nameDisplay = leftPanel.GetNode<UIDisplay>("%NameDisplay");
        this.textureDisplay = leftPanel.GetNode<UIDisplay>("%TextureDisplay");
        this.healthDisplay = leftPanel.GetNode<UIDisplay>("%HealthDisplay");
        this.moneyDisplay = leftPanel.GetNode<UIDisplay>("%MoneyDisplay");
        this.reRollDisplay = leftPanel.GetNode<UIDisplay>("%ReRollDisplay");
        this.deckButton = leftPanel.GetNode<DeckButton>("%DeckButton");
        
        Panel rightPanel = GetNode<Panel>("%RightPanel");
        this.powerUpContainer = rightPanel.GetNode<GridContainer>("%PowerUpContainer");
        this.optionsButton = rightPanel.GetNode<UIButton>("%OptionsButton");
        this.optionsButton.SetData("Options");
        this.optionsButton.button.Pressed += () =>
        {
            UIManager.instance.popUpModel.OpenPopUp("options");
        };
        this.quitButton = rightPanel.GetNode<UIButton>("%QuitButton");
        this.quitButton.SetData("Quit");
        this.quitButton.button.Pressed += () =>
        {
            this.GetTree().Quit();
        };

    }

    public override void Enter()
    {
        Run run = RunManager.instance.currentRun;
        this.nameDisplay.SetData(run.playerData.name);
        this.textureDisplay.SetData(null, run.playerData.texture);
        this.healthDisplay.SetData(run.playerData.health.ToString() + " / " + run.playerData.maxHealth, ResourceManager.instance.HUDIcons[ResourceManager.HUDIconID.HEALTH]);
        this.moneyDisplay.SetData(run.gold.ToString(), ResourceManager.instance.HUDIcons[ResourceManager.HUDIconID.MONEY]);
        this.reRollDisplay.SetData(run.reRolls.ToString(), ResourceManager.instance.HUDIcons[ResourceManager.HUDIconID.REROLL]);
        this.deckButton.SetData(run.playerDeck);
        run.powerUpHandler.SetContainer(this.powerUpContainer);
        run.powerUpHandler.SpawnAllUI();
    }

    public override void Exit()
    {
        Run run = RunManager.instance.currentRun;
        if (run != null)
        {
            run.powerUpHandler.ClearUI();
        }
    }
}
