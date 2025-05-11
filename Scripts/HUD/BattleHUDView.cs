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
    private Action OnEndTurn = null;
    
    public override void _Ready()
    {
        GetSceneNodes();
        ConnectEventSignals();
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
       this.abilityButton.Pressed += ActivateAbility;
       this.endTurnButton = buttonContainer.GetNode<Button>("%EndTurnButton");
       this.endTurnButton.Pressed += EndTurn;

       this.abilityInfoPanel = this.GetNode<AbilityInfoPanel>("%AbilityInfoPanel");
       
       this.genPanel = this.GetNode<Panel>("%GenPanel");
       this.energyLabel = this.genPanel.GetNode<RichTextLabel>("%EnergyLabel");
    }
    
    public void ConnectEventSignals()
    {
        EventManager.instance.ResetAbility += ResetAbility;
    }
    
    public void DisconnectEventSignals()
    {
        EventManager.instance.ResetAbility -= ResetAbility;
    }

    public void SetData(Player player)
    {
        this.player = player;
        this.player.StatsChanged += UpdateStats;
        this.player.deck.CardPileSizeChanged += UpdateDeck;
        this.player.discard.CardPileSizeChanged += UpdateDiscard;

        this.OnEndTurn = this.player.EndTurn;

        if (this.player.playerData.ability.targetType == Character.TargetType.SINGLE)
        {
            this.abilityButton.ToggleMode = true;
        }
        else
        {
            this.abilityButton.ToggleMode = false;
        }

        UpdateStats();
        UpdateDeck();
        UpdateDiscard();
    }

    public void UpdateStats()
    {
        this.healthUI.SetData(this.player.playerData);
        this.abilityInfoPanel.SetData(this.player.playerData);
        this.energyLabel.Text = this.player.energy.ToString();
    }

    public void UpdateDeck()
    {
        this.deckButton.SetData(player.deck);
    }
    
    public void UpdateDiscard()
    {
        this.discardButton.SetData(player.discard, "Discard");
    }

    public override void Enter()
    {
        Run run = RunManager.instance.currentRun;
        run.powerUpHandler.SpawnUI(this.powerUpContainer);
        this.moneyDisplay.SetData(run.gold.ToString(), ResourceManager.instance.HUDIcons["money"]);
        this.mapButton.SetData("Map", ResourceManager.instance.HUDIcons["map"]);
    }

    public override void Exit()
    {
        this.OnEndTurn = null;

        if (this.player != null)
        {
            this.player.StatsChanged -= UpdateStats;
            this.player.deck.CardPileSizeChanged -= UpdateDeck;
            this.player.discard.CardPileSizeChanged -= UpdateDiscard;
            this.player = null;
        }
    }

    public void ActivateAbility()
    {
        if (this.player != null)
        {
            Ability ability = this.player.playerData.ability;

            if (ability.targetType != Character.TargetType.SINGLE)
            {
                GD.Print("Activating ability");
                ability.ApplyEffects(this.player.GetTargets(ability.targetType), this.player.playerData);
            }
            else
            {
                EventManager.instance.EmitSignal(EventManager.SignalName.AbilityAimStarted, this.player);
            }

            this.abilityButton.Disabled = true;
        }
    }

    public void ResetAbility()
    {
        this.abilityButton.Disabled = false;
    }

    public void EndTurn()
    {
        this.OnEndTurn?.Invoke();
    }

    void OnPlayAreaEntered(Area2D area)
    {
        // if (area.IsInGroup("Card"))
        // {
        //     this.energyPanel.AddThemeStyleboxOverride("panel", this.glowStylebox);
        // }
    }
    void OnPlayAreaExited(Area2D area)
    {
        //this.energyPanel.AddThemeStyleboxOverride("panel", this.defaultStylebox);
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
