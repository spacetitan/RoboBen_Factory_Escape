using Godot;
using System;

public partial class GameOverModel : UIModel
{
    private RichTextLabel titleLabel = null;
    private TextureRect texture = null;
    private HealthUI healthUI = null;
    private UIDisplay moneyDisplay = null;
    private UIButton deckButton = null;
    private RichTextLabel summaryLabel = null;
    private ScrollContainer scrollContainer = null;
    private GridContainer gridContainer = null;
    private UIButton closeButton = null;
    
    private Vector2 panelSize = Vector2.Zero;

    public override void _Ready()
    {
        GetSceneNodes();
    }

    private void GetSceneNodes()
    {
        this.titleLabel = this.GetNode<RichTextLabel>("%TitleLabel");
        this.texture = this.GetNode<TextureRect>("%SummaryTexture");
        this.healthUI = this.GetNode<HealthUI>("%HealthUI");
        this.moneyDisplay = this.GetNode<UIDisplay>("%MoneyDisplay");
        this.deckButton = this.GetNode<UIButton>("%DeckButton");
        this.deckButton.button.Pressed += () =>
        {
            UIManager.instance.popUpModel.OpenDeckPopUp(RunManager.instance.currentRun.playerDeck, "Deck");
        };
        this.summaryLabel = this.GetNode<RichTextLabel>("%SummaryLabel");
        this.scrollContainer = this.GetNode<ScrollContainer>("%SumScrollContainer");
        this.gridContainer = this.GetNode<GridContainer>("%SumGridContainer");
        this.closeButton = this.GetNode<UIButton>("%CloseButton");
        this.closeButton.SetData("Back to Title menu ->");
        this.closeButton.button.Pressed += () =>
        {
            UIManager.instance.ChangeStateTo(UIManager.UIState.START);
        };
    }

    public override void Enter()
    {
        UIManager.instance.vfxModel.OpenCurtain();

        Run run = RunManager.instance.currentRun;

        this.texture.Texture = run.playerData.texture;
        this.healthUI.SetData(run.playerData);
        this.moneyDisplay.SetData(run.gold.ToString(), ResourceManager.instance.HUDIcons[ResourceManager.HUDIconID.MONEY]);
        this.deckButton.SetData("Deck", ResourceManager.instance.HUDIcons[ResourceManager.HUDIconID.CARD]);

        this.summaryLabel.Text = "Summary";

        run.powerUpHandler.SetContainer(this.gridContainer);
        run.powerUpHandler.SpawnAllUI();
    }

    public override void Exit()
    {
        
    }
}
