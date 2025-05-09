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
    private Button optionsButton = null;
    private Button quitButton = null;
    
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
        this.optionsButton = rightPanel.GetNode<Button>("%OptionsButton");
        this.optionsButton.Pressed += () =>
        {
            UIManager.instance.popUpModel.OpenPopUp("options");
        };
        this.quitButton = rightPanel.GetNode<Button>("%QuitButton");
        this.quitButton.Pressed += () =>
        {
            this.GetTree().Quit();
        };

    }

    public override void Enter()
    {
        Run run = RunManager.instance.currentRun;
        this.nameDisplay.SetData(run.playerData.name);
        this.textureDisplay.SetData(null, run.playerData.texture);
        this.healthDisplay.SetData(run.playerData.health.ToString() + " / " + run.playerData.maxHealth, ResourceManager.instance.HUDIcons["health"]);
        this.moneyDisplay.SetData(run.gold.ToString(), ResourceManager.instance.HUDIcons["money"]);
        this.reRollDisplay.SetData(run.reRolls.ToString(), ResourceManager.instance.HUDIcons["reRoll"]);
        this.deckButton.SetData(run.playerDeck);
        run.powerUpHandler.SpawnUI(this.powerUpContainer);
    }
}
