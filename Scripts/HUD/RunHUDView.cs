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

    private ScrollContainer scrollContainer = null;
    private GridContainer powerUpContainer = null;
    private UIButton upButton = null;
    private UIButton downButton = null;
    
    private UIButton optionsButton = null;
    private UIButton quitButton = null;
    
    private Vector2 powerUpSize = Vector2.Zero;
    
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
        this.scrollContainer = rightPanel.GetNode<ScrollContainer>("%PowerUpScrollContainer");
        this.powerUpContainer = rightPanel.GetNode<GridContainer>("%PowerUpContainer");
        this.upButton = rightPanel.GetNode<UIButton>("%UpButton");
        this.upButton.button.Pressed += () =>
        {
            this.scrollContainer.SetVScroll(this.scrollContainer.GetVScroll() - (int)this.powerUpSize.Y);
        };
        this.upButton.button.Disabled = true;
        this.downButton = rightPanel.GetNode<UIButton>("%DownButton");
        this.downButton.button.Pressed += () =>
        {
            this.scrollContainer.SetVScroll(this.scrollContainer.GetVScroll() + (int)this.powerUpSize.Y);
        };
        this.downButton.button.Disabled = true;
        this.optionsButton = rightPanel.GetNode<UIButton>("%OptionsButton");
        this.optionsButton.SetData("Options");
        this.optionsButton.button.Pressed += () =>
        {
            UIManager.instance.popUpModel.OpenPopUp(UIModel.ViewID.OPTIONS);
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
        this.powerUpSize = run.powerUpHandler.powerUpSize;

        if (run.powerUpHandler.powerUpUIs.Count > 8)
        {
            this.upButton.button.Disabled = false;
            this.upButton.SetData(null, ResourceManager.instance.HUDIcons[ResourceManager.HUDIconID.ARROW_UP]);
            this.downButton.button.Disabled = false;
            this.downButton.SetData(null, ResourceManager.instance.HUDIcons[ResourceManager.HUDIconID.ARROW_DOWN]);
        }
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
