using Godot;
using System.Collections.Generic;

public partial class PlayView : UIView
{
    private PackedScene playInfoPanelScene = ResourceLoader.Load<PackedScene>("res://Scenes/Start/play_info_panel.tscn");
    private List<PlayInfoPanel> infoPanels = new List<PlayInfoPanel>();
    
    private PackedScene charPickerPanelScene = ResourceLoader.Load<PackedScene>("res://Scenes/Start/char_picker_panel.tscn");
    
    private Button newRunButton;
    private Button continueButton;
    private Button startButton;
    private Button cancelButton;
    
    private HBoxContainer InfoContainer = null;
    private HBoxContainer pickerContainer = null;
    
    private PlayerData playerData = null;
    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        this.newRunButton = this.GetNode<Button>("%NewRunButton");
        this.newRunButton.ButtonPressed = true;
        this.newRunButton.Pressed += () =>
        {
            
        };
        
        this.continueButton = this.GetNode<Button>("%ContinueButton");
        this.continueButton.Pressed += () =>
        {
            
        };
        this.continueButton.SetDisabled(true);
        
        this.startButton = this.GetNode<Button>("%StartButton");
        this.startButton.Pressed += () =>
        {
            if (this.newRunButton.ButtonPressed)
            {
                if (this.playerData != null)
                {
                    RunManager.instance.NewRun(this.playerData);
                }
            }
            else if (this.continueButton.ButtonPressed)
            {
                
            }
        };
        this.startButton.SetDisabled(true);
        
        this.cancelButton = this.GetNode<Button>("%CancelButton");
        this.cancelButton.Pressed += () =>
        {
            this.HideView();
        };
        
        this.InfoContainer = this.GetNode<Panel>("%PlayInfoPanel").GetNode<HBoxContainer>("InfoContainer");
        PlayInfoPanel infoPanel = this.playInfoPanelScene.Instantiate() as PlayInfoPanel;
        infoPanel.GetSceneNodes();
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
        foreach (KeyValuePair<StringName, PlayerData> kvp in ResourceManager.instance.characters)
        {
            CharPickerPanel pickerPanel = charPickerPanelScene.Instantiate() as CharPickerPanel;
            pickerPanel.GetSceneNodes();
            pickerPanel.SetData(kvp.Value);
            pickerPanel.pickerButton.Pressed += () =>
            {
                SetInfoPanels(kvp.Value);
                this.playerData = kvp.Value;
                this.startButton.SetDisabled(false);
            };
            this.pickerContainer.AddChild(pickerPanel);
        }
    }

    public void SetInfoPanels(PlayerData data)
    {
        this.infoPanels[0].SetData(new PlayInfoPanel.InfoData()
        {
            texture = data.texture,
            title = data.name,
            body = data.id,
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
        this.newRunButton.ButtonPressed = true;
        this.startButton.SetDisabled(true);
        ResetInfoPanels();
    }
}
