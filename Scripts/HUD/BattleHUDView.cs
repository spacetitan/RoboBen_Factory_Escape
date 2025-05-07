using Godot;
using System;

public partial class BattleHUDView : UIView
{
    private HBoxContainer powerUpContainer = null;
    private UIDisplay moneyDisplay = null;
    private Button optionsButton = null;
    private Button quitButton = null;
    
    private HealthUI healthUI = null;
    private DeckButton discardButton = null;
    
    private DeckButton deckButton = null;
    private UIButton mapButton = null;
    
    private Button abilityButton = null;
    private Button endTurnButton = null;
    private AbilityInfoPanel abilityInfoPanel = null;
    
    private Panel genPanel = null;
    private RichTextLabel energyLabel = null;
    
    private GridContainer statusContainer = null;
    
    private Player player = null;
    
    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
       Panel topPanel = GetNode<Panel>("%TopPanel");
       this.powerUpContainer = topPanel.GetNode<HBoxContainer>("%PowerUpContainer");
       this.moneyDisplay = topPanel.GetNode<UIDisplay>("%MoneyDisplay");
       this.optionsButton = topPanel.GetNode<Button>("%OptionsButton");
       this.optionsButton.Pressed += () =>
       {
           UIManager.instance.popUpModel.OpenPopUp("options");
       };
       this.quitButton = topPanel.GetNode<Button>("%QuitButton");
       this.quitButton.Pressed += () =>
       {
           this.GetTree().Quit();
       };
       
       Panel leftPanel = GetNode<Panel>("%LeftPanel");
       this.discardButton = leftPanel.GetNode<DeckButton>("%DiscardButton");
       this.healthUI = leftPanel.GetNode<HealthUI>("%HealthUI");
       
       Panel rightPanel = GetNode<Panel>("%RightPanel");
       this.deckButton = rightPanel.GetNode<DeckButton>("%DeckButton");
       this.mapButton = rightPanel.GetNode<UIButton>("%MapButton");

       HBoxContainer buttonContainer = this.GetNode<HBoxContainer>("%ButtonContainer");
       this.abilityButton = buttonContainer.GetNode<Button>("%AbilityButton");
       this.endTurnButton = buttonContainer.GetNode<Button>("%EndTurnButton");

       this.abilityInfoPanel = this.GetNode<AbilityInfoPanel>("%AbilityInfoPanel");
       
       this.genPanel = this.GetNode<Panel>("%GenPanel");
       this.energyLabel = this.genPanel.GetNode<RichTextLabel>("%EnergyLabel");
    }

    public void SetData(Player player)
    {
        this.player = player;
        player.StatsChanged += UpdateStats;
        UpdateStats();
    }

    public void UpdateStats()
    {
        this.healthUI.SetData(this.player.playerData);
        this.abilityInfoPanel.SetData(this.player.playerData);
        this.energyLabel.Text = this.player.energy.ToString();
    }

    public override void Enter()
    {
        Run run = RunManager.instance.currentRun;
        run.powerUpHandler.SpawnUI(this.powerUpContainer);
        this.moneyDisplay.SetData(run.gold.ToString(), ResourceManager.instance.HUDIcons["money"]);
        this.mapButton.SetData("Map", ResourceManager.instance.HUDIcons["map"]);
    }

    public void OnAbilityMouseEntered()
    {
        this.abilityInfoPanel.ShowView();
    }

    public void OnAbilityMouseExited()
    {
        this.abilityInfoPanel.HideView();
    }
}
