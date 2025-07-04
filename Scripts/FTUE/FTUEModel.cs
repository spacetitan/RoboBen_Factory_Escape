using Godot;
using System;
using System.Collections.Generic;

public partial class FTUEModel : UIModel
{
    private TextureRect mask = null;
    
    private Control rightPanel = null;
    private RichTextLabel rightLabel = null;
    private UIButton rightButton = null;
    
    private Control leftPanel = null;
    private RichTextLabel leftLabel = null;
    private UIButton leftButton = null;
    
    private Control cardPanel = null;
    private CardDisplayUI exampleCard = null;
    private CardDisplayUI DisabledCard = null;

    private Control playPanels = null;

    private TextureRect arrow = null;
    private TextureRect arrow2 = null;

    private int currentStep = 0;
    private List<string> textStep = new List<string>()
    {
        "Welcome!",
        "Drag cards onto the energy tray to generate energy based on the cards Gen stat.",
        "Use energy to play cards. Cards will become clearer when you have enough energy to play them.",
        "Each Character has an ability that you can use on your turn!",
        "You also have power-ups that will activate passively on your turn.",
        "Good luck!"
    };

    private List<Vector2> maskStep = new List<Vector2>()
    {
        new Vector2(3.2f, -.86f), //fill amount .75, scalex 0.5f
        new Vector2(0, 0),
        new Vector2(0, 0),
        new Vector2(0, 0),
        new Vector2(0, 0),
    };
    
    private Action onComplete = null;
    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        this.mask = this.GetNode<TextureRect>("%Mask");
        
        this.rightPanel = this.GetNode<Control>("%RightPanel");
        this.rightLabel = this.rightPanel.GetNode<RichTextLabel>("%RightLabel");
        this.rightButton = this.rightPanel.GetNode<UIButton>("%RightButton");
        this.rightButton.SetData("Continue");
        this.rightButton.button.Pressed += OnButtonClicked;
        
        this.leftPanel = this.GetNode<Control>("%LeftPanel");
        this.leftLabel = this.leftPanel.GetNode<RichTextLabel>("%LeftLabel");
        this.leftButton = this.leftPanel.GetNode<UIButton>("%LeftButton");
        this.leftButton.SetData("Continue");
        this.leftButton.button.Pressed += OnButtonClicked;
        
        this.cardPanel = this.GetNode<Control>("%CardPanel");
        
        this.exampleCard = this.GetNode<CardDisplayUI>("%ExampleCard");
        this.exampleCard.GetSceneNodes();
        this.exampleCard.SetData(ResourceManager.instance.cards[CardData.CardID.SNEAKATTACK], null, true, false);
        
        this.DisabledCard = this.GetNode<CardDisplayUI>("%DisabledCard");
        this.DisabledCard.GetSceneNodes();
        this.DisabledCard.SetData(ResourceManager.instance.cards[CardData.CardID.SNEAKATTACK], null, true, false);
        this.DisabledCard.DisableCard();
        
        this.playPanels = this.GetNode<Control>("%PlayPanels");
        
        this.arrow = this.GetNode<TextureRect>("%ArrowRect");
        this.arrow2 = this.GetNode<TextureRect>("%ArrowRect2");
    }

    public void SetData(Action action)
    {
        this.onComplete = action;
        this.rightPanel.Show();
        this.rightLabel.Text = this.textStep[0];
        
        this.leftPanel.Hide();
    }

    private void OnButtonClicked()
    {
        this.currentStep++;

        if (this.currentStep >= this.textStep.Count)
        {
            UIManager.instance.ftueModel.HideModel();
            this.onComplete?.Invoke();
            return;
        }

        this.rightLabel.Text = this.textStep[this.currentStep];
        this.leftLabel.Text = this.textStep[this.currentStep];
        
        ShaderMaterial shader = this.mask.GetMaterial() as ShaderMaterial;

        switch (this.currentStep)
        {
            case 1:
                this.rightLabel.Text = this.textStep[this.currentStep];
                this.cardPanel.Show();
                
                shader.SetShaderParameter("fill_amount", 0.75);
                shader.SetShaderParameter("mask_offset", new Vector2(3.2f, -0.86f));
                shader.SetShaderParameter("mask_size", new Vector2(0.5f, 1.0f));
                break;
            
            case 2:
                this.rightLabel.Text = this.textStep[this.currentStep];
                this.arrow.Hide();
                this.arrow2.Show();
                this.DisabledCard.Show();
                this.playPanels.Show();
                
                shader.SetShaderParameter("fill_amount", 1.0);
                shader.SetShaderParameter("mask_offset", new Vector2(0f, 0f));
                shader.SetShaderParameter("mask_size", new Vector2(1.0f, 1.0f));
                break;
            
            case 3:
                this.rightPanel.Hide();
                this.cardPanel.Hide();
                this.leftPanel.Show();
                this.leftLabel.Text = this.textStep[this.currentStep];
                
                shader.SetShaderParameter("fill_amount", .85);
                shader.SetShaderParameter("mask_offset", new Vector2(-1.875f, -1.75f));
                break;
            
            case 4:
                this.leftLabel.Text = this.textStep[this.currentStep];
                shader.SetShaderParameter("fill_amount", .845);
                shader.SetShaderParameter("mask_offset", new Vector2(.288f, 2.7f));
                shader.SetShaderParameter("mask_size", new Vector2(3.7f, 1.0f));
                break;
            
            case 5:
                this.leftLabel.Text = this.textStep[this.currentStep];
                shader.SetShaderParameter("fill_amount", 1.0);
                shader.SetShaderParameter("mask_offset", new Vector2(0f, 0f));
                shader.SetShaderParameter("mask_size", new Vector2(1.0f, 1.0f));
                break;
            
            case 6:
                break;
            
            
            default:
                break;
        }
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        this.onComplete = null;
        this.currentStep = 0;
        
        this.leftPanel.Hide();
        this.rightPanel.Show();
        
        this.arrow.Show();
        this.arrow2.Hide();
        this.DisabledCard.Hide();
        this.playPanels.Hide();
        
    }
}
