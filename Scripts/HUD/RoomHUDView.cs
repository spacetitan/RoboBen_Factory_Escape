using Godot;
using System;

public partial class RoomHUDView : UIView
{
    private HBoxContainer powerUpContainer = null;
    private UIDisplay moneyDisplay = null;
    private UIButton optionsButton = null;
    private UIButton quitButton = null;
    
    private HealthUI healthUI = null;
    private DeckButton deckButton = null;
    private UIButton mapButton = null;
    
    private UIButton leaveButton = null;
    
    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        Panel topPanel = GetNode<Panel>("%TopPanel");
        this.powerUpContainer = topPanel.GetNode<HBoxContainer>("%PowerUpContainer");
        this.moneyDisplay = topPanel.GetNode<UIDisplay>("%MoneyDisplay");
        this.optionsButton = topPanel.GetNode<UIButton>("%OptionsButton");
        this.optionsButton.SetData("Options");
        this.optionsButton.button.Pressed += () => { UIManager.instance.popUpModel.OpenPopUp("options"); };
        this.quitButton = topPanel.GetNode<UIButton>("%QuitButton");
        this.quitButton.SetData("Quit");
        this.quitButton.button.Pressed += () => { this.GetTree().Quit(); };
        
        Panel bottomPanel = GetNode<Panel>("%BottomPanel");
        this.healthUI = bottomPanel.GetNode<HealthUI>("%HealthUI");
        this.deckButton = bottomPanel.GetNode<DeckButton>("%DeckButton");
        this.mapButton = bottomPanel.GetNode<UIButton>("%MapButton");
        this.mapButton.SetData("Map", ResourceManager.instance.HUDIcons["map"]);

        this.leaveButton = this.GetNode<UIButton>("%LeaveButton");
        this.leaveButton.SetData("Leave");
        this.leaveButton.button.Pressed += () => { UIManager.instance.ChangeStateTo(UIManager.UIState.RUN); };
    }

    public override void Enter()
    {
        Run run = RunManager.instance.currentRun;
        
        run.powerUpHandler.SetContainer(this.powerUpContainer);
        run.powerUpHandler.SpawnAllUI();
        
        this.moneyDisplay.SetData(run.gold.ToString(), ResourceManager.instance.HUDIcons["money"]);
        this.healthUI.SetData(run.playerData);
        this.deckButton.SetData(run.playerDeck);
    }

    public void UpdateHealthUI()
    {
        this.healthUI.SetData(RunManager.instance.currentRun.playerData);
        this.moneyDisplay.SetData(RunManager.instance.currentRun.gold.ToString(), ResourceManager.instance.HUDIcons["money"]);
    }

    public void UpdateDeckButton()
    {
        this.deckButton.SetData(RunManager.instance.currentRun.playerDeck);
    }
}
