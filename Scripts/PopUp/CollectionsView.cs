using Godot;
using System;
using System.Collections.Generic;

public partial class CollectionsView : UIView
{
    private StyleBox charPickerStyleBox = ResourceLoader.Load<StyleBox>("res://Themes/Start/CharPicker/ColCharPickStyleBox.tres");
    
    private ScrollContainer scrollContainer = null;
    private GridContainer gridContainer = null;
 
    private HBoxContainer buttonContainer = null;
    private List<UIButton> headerButtons = new List<UIButton>();
    
    private TextureRect textureRect = null;
    private RichTextLabel titleLabel = null;
    private RichTextLabel bodyLabel = null;
    
    private UIButton closeButton = null;
    
    private Vector2 panelSize = Vector2.Zero;

    public override void _Ready()
    {
        GetSceneNodes();
    }

    private void GetSceneNodes()
    {
        this.scrollContainer = this.GetNode<ScrollContainer>("%ColScrollContainer");
        this.gridContainer = this.GetNode<GridContainer>("%ColGridContainer");

        this.panelSize = this.scrollContainer.GetSize();
        
        ButtonGroup buttonGroup = new ButtonGroup();
        this.buttonContainer = this.GetNode<HBoxContainer>("%ButtonContainer");
        foreach (UIButton button in buttonContainer.GetChildren())
        {
            button.GetSceneNodes();
            button.button.ToggleMode = true;
            button.button.ButtonGroup = buttonGroup;
            this.headerButtons.Add(button);
        }
        
        this.headerButtons[0].SetData("Characters");
        this.headerButtons[0].button.Pressed += () =>
        {
            ClearData();

            foreach (var VARIABLE in ResourceManager.instance.characters)
            {
                CharPickerPanel panel = ResourceManager.instance.charPickerPanelScene.Instantiate() as CharPickerPanel;
                panel.SetCustomMinimumSize(new Vector2(this.panelSize.X / 4.5f, this.panelSize.X / 4.5f));
                panel.GetSceneNodes();
                panel.SetData(VARIABLE.Value);
                panel.AddThemeStyleboxOverride("panel", this.charPickerStyleBox);
                panel.pickerButton.Pressed += () =>
                {
                    this.titleLabel.Text = panel.playerData.name;
                    this.bodyLabel.Text = panel.playerData.desc;
                    this.textureRect.Texture = panel.playerData.texture;
                };
                this.gridContainer.AddChild(panel);
            }
        };
        
        this.headerButtons[1].SetData("Enemies");
        this.headerButtons[1].button.Pressed += () =>
        {
            ClearData();

            foreach (var VARIABLE in ResourceManager.instance.enemies)
            {
                CharPickerPanel panel = ResourceManager.instance.charPickerPanelScene.Instantiate() as CharPickerPanel;
                panel.SetCustomMinimumSize(new Vector2(this.panelSize.X / 4.5f, this.panelSize.X / 4.5f));
                panel.GetSceneNodes();
                panel.AddThemeStyleboxOverride("panel", this.charPickerStyleBox);
                panel.SetData(VARIABLE.Value);
                panel.pickerButton.Pressed += () =>
                {
                    this.titleLabel.Text = panel.enemyData.name;
                    this.bodyLabel.Text = panel.enemyData.desc;
                    this.textureRect.Texture = panel.enemyData.texture;
                };
                this.gridContainer.AddChild(panel);
            }   
        };
        
        this.headerButtons[2].SetData("Cards");
        this.headerButtons[2].button.Pressed += () =>
        {
            ClearData();

            foreach (var VARIABLE in ResourceManager.instance.cards)
            {
                CardDisplayUI cardUI = ResourceManager.instance.displayCard.Instantiate() as CardDisplayUI;
                cardUI.SetCustomMinimumSize(new Vector2(this.panelSize.X / 4.5f, (this.panelSize.X / 4.5f) * 1.4f));
                cardUI.GetSceneNodes();
                cardUI.SetData(VARIABLE.Value, () =>
                {
                    this.titleLabel.Text = cardUI.cardData.cardName;
                    this.bodyLabel.Text = cardUI.cardData.GetDefaultToolip();
                    this.textureRect.Texture = cardUI.cardData.Texture;
                });
                this.gridContainer.AddChild(cardUI);
            }
        };
        
        this.headerButtons[3].SetData("Power-Ups");
        this.headerButtons[3].button.Pressed += () =>
        {
            ClearData();

            foreach (var VARIABLE in ResourceManager.instance.powerUps)
            {
                CardDisplayUI cardUI = ResourceManager.instance.displayCard.Instantiate() as CardDisplayUI;
                cardUI.SetCustomMinimumSize(new Vector2(this.panelSize.X / 4.5f, (this.panelSize.X / 4.5f) * 1.4f));
                cardUI.GetSceneNodes();
                cardUI.SetData(VARIABLE.Value, () =>
                {
                    this.titleLabel.Text = cardUI.powerUp.name;
                    this.bodyLabel.Text = cardUI.powerUp.description;
                    this.textureRect.Texture = cardUI.powerUp.texture;
                });
                this.gridContainer.AddChild(cardUI);
            }
        };

        this.textureRect = this.GetNode<TextureRect>("%InfoTexture");
        this.titleLabel = this.GetNode<RichTextLabel>("%TitleLabel");
        this.bodyLabel = this.GetNode<RichTextLabel>("%BodyLabel");
        
        this.closeButton = this.GetNode<UIButton>("%CloseButton");
        this.closeButton.SetData("Close");
        this.closeButton.button.Pressed += () => { UIManager.instance.popUpModel.ClosePopup(UIModel.ViewID.COLLECTIONS); };
    }

    public override void Enter()
    {
        this.headerButtons[0].button.ButtonPressed = true;
        foreach (var VARIABLE in ResourceManager.instance.characters)
        {
            CharPickerPanel panel = ResourceManager.instance.charPickerPanelScene.Instantiate() as CharPickerPanel;
            panel.SetCustomMinimumSize(new Vector2(this.panelSize.X / 4.5f, this.panelSize.X / 4.5f));
            panel.GetSceneNodes();
            panel.AddThemeStyleboxOverride("panel", this.charPickerStyleBox);
            panel.SetData(VARIABLE.Value);
            panel.pickerButton.Pressed += () =>
            {
                this.titleLabel.Text = panel.playerData.name;
                this.bodyLabel.Text = panel.playerData.desc;
                this.textureRect.Texture = panel.playerData.texture;
            };
            this.gridContainer.AddChild(panel);
        }
    }

    public void ClearData()
    {
        foreach (var VARIABLE in this.gridContainer.GetChildren())
        {
            VARIABLE.QueueFree();
        }

        this.titleLabel.Text = "";
        this.bodyLabel.Text = "";
        this.textureRect.Texture = ResourceManager.instance.debugIcon;
        
        Tween tween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Linear);
        tween.TweenProperty(this.scrollContainer, "scroll_vertical", 0, .01f);
    }

    public override void Exit()
    {
        ClearData();
    }
}
