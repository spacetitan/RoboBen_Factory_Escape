using Godot;
using System;

public partial class BattleHUDView : UIView
{
    private StyleBox genPanelStyleBox = ResourceLoader.Load<StyleBox>("res://Themes/Battle/GenPanel.tres");
    private StyleBox genGlowPanelStyleBox = ResourceLoader.Load<StyleBox>("res://Themes/Battle/GenGlowPanel.tres");
    
    private HBoxContainer powerUpContainer = null;
    private UIDisplay moneyDisplay = null;
    private UIButton optionsButton = null;
    private UIButton quitButton = null;
    
    private HealthUI healthUI = null;
    private DeckButton discardButton = null;
    
    private DeckButton deckButton = null;
    private UIButton mapButton = null;
    
    private UIButton abilityButton = null;
    private UIButton endTurnButton = null;
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
       this.optionsButton = topPanel.GetNode<UIButton>("%OptionsButton");
       this.optionsButton.SetData("Options");
       this.optionsButton.button.Pressed += () =>
       {
           UIManager.instance.popUpModel.OpenPopUp("options");
       };
       this.quitButton = topPanel.GetNode<UIButton>("%QuitButton");
       this.quitButton.SetData("Quit");
       this.quitButton.button.Pressed += () =>
       {
           this.GetTree().Quit();
       };
       
       Panel leftPanel = GetNode<Panel>("%LeftPanel");
       this.discardButton = leftPanel.GetNode<DeckButton>("%DiscardButton");
       this.healthUI = leftPanel.GetNode<HealthUI>("%HealthUI");
       
       Panel rightPanel = GetNode<Panel>("%RightPanel");
       this.deckButton = rightPanel.GetNode<DeckButton>("%DeckButton");
       this.mapButton = rightPanel.GetNode<UIButton>("%MapButton");
       this.mapButton.SetData("Map", ResourceManager.instance.HUDIcons[ResourceManager.HUDIconID.MAP]);
       this.mapButton.button.Pressed += () => { UIManager.instance.popUpModel.OpenMapPopUp(); };

       HBoxContainer buttonContainer = this.GetNode<HBoxContainer>("%ButtonContainer");
       this.abilityButton = buttonContainer.GetNode<UIButton>("%AbilityButton");
       this.abilityButton.SetData("Ability");
       this.abilityButton.button.Pressed += ActivateAbility;
       this.endTurnButton = buttonContainer.GetNode<UIButton>("%EndTurnButton");
       this.endTurnButton.SetData("End Turn");
       this.endTurnButton.button.Pressed += EndTurn;

       this.abilityInfoPanel = this.GetNode<AbilityInfoPanel>("%AbilityInfoPanel");
       
       this.genPanel = this.GetNode<Panel>("%GenPanel");
       this.energyLabel = this.genPanel.GetNode<RichTextLabel>("%EnergyLabel");
    }
    
    public void ConnectEventSignals()
    {
        EventManager.instance.PlayerTurnStarted += ResetAbility;
    }
    
    public void DisconnectEventSignals()
    {
        EventManager.instance.PlayerTurnStarted -= ResetAbility;
    }

    public void SetData(Player player)
    {
        this.player = player;
        this.player.statusHandler.SetContainer(this.statusContainer);
        this.player.StatsChanged += UpdateStats;
        this.player.deck.CardPileSizeChanged += UpdateDeck;
        this.player.discard.CardPileSizeChanged += UpdateDiscard;

        this.OnEndTurn = this.player.EndTurn;

        if (this.player.playerData.ability.targetType == Character.TargetType.SINGLE)
        {
            this.abilityButton.button.ToggleMode = true;
        }
        else
        {
            this.abilityButton.button.ToggleMode = false;
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

    public void UpdateMoney()
    {
        this.moneyDisplay.SetData(RunManager.instance.currentRun.gold.ToString(), ResourceManager.instance.HUDIcons[ResourceManager.HUDIconID.MONEY]);
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
        run.powerUpHandler.SetContainer(this.powerUpContainer);
        run.powerUpHandler.SpawnAllUI();
        this.moneyDisplay.SetData(run.gold.ToString(), ResourceManager.instance.HUDIcons[ResourceManager.HUDIconID.MONEY]);
        this.mapButton.SetData("Map", ResourceManager.instance.HUDIcons[ResourceManager.HUDIconID.MAP]);
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

            this.abilityButton.button.Disabled = true;
        }
    }

    public void ResetAbility()
    {
        this.abilityButton.button.Disabled = false;
    }

    public void EndTurn()
    {
        this.OnEndTurn?.Invoke();
    }

    void OnGenAreaEntered(Area2D area)
    {
        if (area.IsInGroup("Card") || area.IsInGroup("Targeter"))
        {
            this.genPanel.AddThemeStyleboxOverride("panel", this.genGlowPanelStyleBox);
        }
    }
    void OnGenAreaExited(Area2D area)
    {
        this.genPanel.AddThemeStyleboxOverride("panel", this.genPanelStyleBox);
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
