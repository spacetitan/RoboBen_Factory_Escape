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
    private UIButton upButton = null;
    private UIButton downButton = null;
    private UIButton closeButton = null;
    
    private Vector2 powerUpSize = Vector2.Zero;

    public bool win { get; private set; } = false;

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
        this.upButton = this.GetNode<UIButton>("%UpButton");
        this.upButton.button.Pressed += () =>
        {
            this.scrollContainer.SetVScroll(this.scrollContainer.GetVScroll() - (int)this.powerUpSize.Y);
        };
        this.upButton.button.Disabled = true;
        this.downButton = this.GetNode<UIButton>("%DownButton");
        this.downButton.button.Pressed += () =>
        {
            this.scrollContainer.SetVScroll(this.scrollContainer.GetVScroll() + (int)this.powerUpSize.Y);
        };
        this.downButton.button.Disabled = true;
        this.gridContainer = this.GetNode<GridContainer>("%SumGridContainer");
        this.closeButton = this.GetNode<UIButton>("%CloseButton");
        this.closeButton.SetData("Back to Title menu ->");
        this.closeButton.button.Pressed += () =>
        {
            UIManager.instance.ChangeStateTo(UIManager.UIState.START);
        };
    }

    public void SetData(bool win)
    {
        Run run = RunManager.instance.currentRun;

        this.texture.Texture = run.playerData.texture;
        this.healthUI.SetData(run.playerData);
        this.moneyDisplay.SetData(run.gold.ToString(), ResourceManager.instance.HUDIcons[ResourceManager.HUDIconID.MONEY]);
        this.deckButton.SetData("Deck", ResourceManager.instance.HUDIcons[ResourceManager.HUDIconID.CARD]);

        this.summaryLabel.Text = "Enemies slain: " + run.stats.enemySlain;
        this.summaryLabel.Text += "\nMoney spent: " + run.stats.moneySpent;
        this.summaryLabel.Text += "\nRe-Rolls used: " + run.stats.reRollsUsed; 

        run.powerUpHandler.SetContainer(this.gridContainer);
        run.powerUpHandler.SpawnAllUI(UIManager.UIState.GAMEOVER);
        this.powerUpSize = run.powerUpHandler.powerUpSize;
        if (run.powerUpHandler.powerUpUIs.Count > 8)
        {
            this.upButton.button.Disabled = false;
            this.upButton.SetData(null, ResourceManager.instance.HUDIcons[ResourceManager.HUDIconID.ARROW_UP]);
            this.downButton.button.Disabled = false;
            this.downButton.SetData(null, ResourceManager.instance.HUDIcons[ResourceManager.HUDIconID.ARROW_DOWN]);
        }
        
        this.win = win;
        if (win)
        {
            this.titleLabel.Text = "You win!";
        }
        else
        {
            this.titleLabel.Text = "Try again!";
        }
    }

    public override void Enter()
    {
        UIManager.instance.vfxModel.OpenCurtain(() =>
        {
            if (!win)
            {
                AudioManager.instance.sfxPlayer.Play(ResourceManager.instance.audio[ResourceManager.AudioID.BATTLE_LOSE], true);
            }
        });
        
        RunManager.instance.ClearRun();
        GameManager.instance.DeleteSaveData();
    }

    public override void Exit()
    {
        
    }
}
