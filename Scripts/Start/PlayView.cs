using Godot;
using System.Collections.Generic;

public partial class PlayView : UIView
{
    private PackedScene playInfoPanelScene = ResourceLoader.Load<PackedScene>("res://Scenes/Start/play_info_panel.tscn");
    private List<PlayInfoPanel> infoPanels = new List<PlayInfoPanel>();
    
    private UIButton newRunButton;
    private UIButton continueButton;
    private UIButton startButton;
    private UIButton cancelButton;
    
    private HBoxContainer InfoContainer = null;
    private HBoxContainer pickerContainer = null;
    
    private PlayerData playerData = null;
    
    private Vector2 infoSize = Vector2.Zero;
    private Vector2 pickerSize = Vector2.Zero;
    
    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        ButtonGroup buttonGroup = new ButtonGroup();
        
        this.newRunButton = this.GetNode<UIButton>("%NewRunButton");
        this.newRunButton.SetData("New Run");
        this.newRunButton.button.ToggleMode = true;
        this.newRunButton.button.ButtonPressed = true;
        this.newRunButton.button.ButtonGroup = buttonGroup;
        this.newRunButton.button.Pressed += () =>
        {
            this.playerData = null;
            ResetInfoPanels();
            this.continueButton.button.SetPressed(false);
        };
        
        this.continueButton = this.GetNode<UIButton>("%ContinueButton");
        this.continueButton.SetData("Continue");
        this.continueButton.button.ToggleMode = true;
        this.continueButton.button.ButtonGroup = buttonGroup;
        this.continueButton.button.Pressed += () =>
        {
            this.startButton.button.SetDisabled(false);
            this.playerData = null;
            int id = (int) GameManager.instance.GetLoadData("Player Data")["ID"];
            SetInfoPanels(ResourceManager.instance.characters[(CharacterData.CharacterID)id]);
            this.startButton.button.SetPressed(false);
        };

        this.startButton = this.GetNode<UIButton>("%StartButton");
        this.startButton.SetData("Start!");
        this.startButton.button.Pressed += () =>
        {
            if (this.newRunButton.button.ButtonPressed)
            {
                if (this.playerData != null)
                {
                    RunManager.instance.NewRun(this.playerData);
                }
            }
            else if (this.continueButton.button.ButtonPressed)
            {
                RunManager.instance.ContinueRun(GameManager.instance.GetLoadData());
            }
        };
        this.startButton.button.SetDisabled(true);
        this.startButton.clickSFX = ResourceManager.instance.audio[ResourceManager.AudioID.GAME_START];
        
        this.cancelButton = this.GetNode<UIButton>("%CancelButton");
        this.cancelButton.SetData("Cancel");
        this.cancelButton.button.Pressed += () =>
        {
            this.HideView();
        };
        
        this.InfoContainer = this.GetNode<Panel>("%PlayInfoPanel").GetNode<HBoxContainer>("InfoContainer");
        this.infoSize = new Vector2(this.InfoContainer.Size.X / 3.5f, this.InfoContainer.Size.Y * .8f);
        //GD.Print(this.infoSize);
        
        PlayInfoPanel infoPanel = this.playInfoPanelScene.Instantiate() as PlayInfoPanel;
        infoPanel.GetSceneNodes();
        infoPanel.SetSize(this.infoSize);
        infoPanel.SetCustomMinimumSize(this.infoSize);
        infoPanel.SetData(new PlayInfoPanel.InfoData()
        {
            texture = ResourceManager.instance.debugIcon,
            title = "Character Name",
            body = "Character Details",
            type = "Character"
        });
        this.InfoContainer.AddChild(infoPanel);
        this.infoPanels.Add(infoPanel);
        
        infoPanel = this.playInfoPanelScene.Instantiate() as PlayInfoPanel;
        infoPanel.GetSceneNodes();
        infoPanel.SetCustomMinimumSize(this.infoSize);
        infoPanel.SetData(new PlayInfoPanel.InfoData()
        {
            texture = ResourceManager.instance.debugIcon,
            title = "Ability Name",
            body = "Ability Details",
            type = "Ability"
        });
        this.InfoContainer.AddChild(infoPanel);
        this.infoPanels.Add(infoPanel);
        
        infoPanel = this.playInfoPanelScene.Instantiate() as PlayInfoPanel;
        infoPanel.GetSceneNodes();
        infoPanel.SetCustomMinimumSize(this.infoSize);
        infoPanel.SetData(new PlayInfoPanel.InfoData()
        {
            texture = ResourceManager.instance.debugIcon,
            title = "Power-Up Name",
            body = "Power-Up Details",
            type = "Power-Up"
        });
        this.InfoContainer.AddChild(infoPanel);
        this.infoPanels.Add(infoPanel);
        
        this.pickerContainer = this.GetNode<Panel>("%CharPickerPanel").GetNode<HBoxContainer>("%PickerContainer");
        this.pickerSize = new Vector2(this.pickerContainer.Size.X / 3f, this.pickerContainer.Size.X / 3f);
        foreach (KeyValuePair<CharacterData.CharacterID, PlayerData> kvp in ResourceManager.instance.characters)
        {
            CharPickerPanel pickerPanel = ResourceManager.instance.charPickerPanelScene.Instantiate() as CharPickerPanel;
            pickerPanel.GetSceneNodes();
            pickerPanel.SetData(kvp.Value);
            pickerPanel.SetCustomMinimumSize(this.pickerSize);
            pickerPanel.pickerButton.Pressed += () =>
            {
                SetInfoPanels(kvp.Value);
                this.playerData = kvp.Value;
                this.startButton.button.SetDisabled(false);
            };
            this.pickerContainer.AddChild(pickerPanel);
        }
    }

    public override void Enter()
    {
        if (GameManager.instance.HasLoadFile())
        {
            this.continueButton.button.SetDisabled(false);
        }
        else
        {
            this.continueButton.button.SetDisabled(true);
        }
    }

    public void SetInfoPanels(PlayerData data)
    {
        this.infoPanels[0].SetData(new PlayInfoPanel.InfoData()
        {
            texture = data.texture,
            title = data.name,
            body = data.desc,
            type = "Character"
        });
        
        this.infoPanels[1].SetData(new PlayInfoPanel.InfoData()
        {
            texture = data.ability.texture,
            title = data.ability.name,
            body = data.ability.desc,
            type = "Ability"
        });
        
        this.infoPanels[2].SetData(new PlayInfoPanel.InfoData()
        {
            texture = data.starterPowerUp.texture,
            title = data.starterPowerUp.name,
            body = data.starterPowerUp.description,
            type = "Power-Up"
        });
    }

    public void ResetInfoPanels()
    {
        this.infoPanels[0].SetData(new PlayInfoPanel.InfoData()
        {
            texture = ResourceManager.instance.debugIcon,
            title = "Character Name",
            body = "Character Details",
            type = "Character"
        });
        
        this.infoPanels[1].SetData(new PlayInfoPanel.InfoData()
        {
            texture = ResourceManager.instance.debugIcon,
            title = "Ability Name",
            body = "Ability Details",
            type = "Ability"
        });
        
        this.infoPanels[2].SetData(new PlayInfoPanel.InfoData()
        {
            texture = ResourceManager.instance.debugIcon,
            title = "Power-Up Name",
            body = "Power-Up Details",
            type = "Power-Up"
        });
    }

    public override void Exit()
    {
        if (this.newRunButton != null && this.startButton != null)
        {
            this.newRunButton.button.ButtonPressed = true;
            this.startButton.button.SetDisabled(true);
            ResetInfoPanels();
        }
    }
}
