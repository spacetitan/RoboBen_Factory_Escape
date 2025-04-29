using Godot;
using System.Collections.Generic;

public partial class PlayView : UIView
{
    private PackedScene playInfoPanelScene = ResourceLoader.Load<PackedScene>("res://Scenes/Start/play_info_panel.tscn");
    private List<PlayInfoPanel> infoPanels = new List<PlayInfoPanel>();
    //private PackedScene charPickerPanelScene = ResourceLoader.Load<PackedScene>("res://Scenes/StartMenu/NewRun/character_picker_panel.tscn");
    
    private Button newRunButton;
    private Button continueButton;
    private Button startButton;
    private Button cancelButton;
    
    private HBoxContainer InfoContainer = null;
    private HBoxContainer pickerContainer = null;
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
                // if(this.currentCharacter != null)
                // {
                //     RunManager.instance.NewRun(this.currentCharacter);
                //     UIManager.instance.ChangePanelTo(UIPanelState.RUN);
                // }
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
    }

    public void SetInfoPanels()
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
        
        this.infoPanels[1].SetData(new PlayInfoPanel.InfoData()
        {
            texture = ResourceManager.instance.debugIcon,
            title = "Ability Name",
            body = "Ability Details",
            type = "Ability"
        });
    }

    public override void Exit()
    {
        this.newRunButton.ButtonPressed = true;
        foreach (PlayInfoPanel infoPanel in infoPanels)
        {
            infoPanel.ResetData();
        }
    }
}
