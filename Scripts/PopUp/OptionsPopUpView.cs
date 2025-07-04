using Godot;
using System;

public partial class OptionsPopUpView : UIView
{
    private VBoxContainer optionsContainer = null;

    private Panel musicPanel = null;
    private HSlider musicSlider = null;
    private RichTextLabel musicLabel = null;
    
    private Panel sfxPanel = null;
    private HSlider sfxSlider = null;
    private RichTextLabel sfxLabel = null;
    
    private MenuButton windowModeButton = null;
    private UIButton resetFTUEButton = null;
    
    private UIButton saveButton = null;
    private UIButton closeButton = null;
    
    public override void _Ready()
    {
        GetSceneNodes();
    }

    private void GetSceneNodes()
    {
        this.optionsContainer = this.GetNode<VBoxContainer>("%OptionsContainer");
        
        this.musicPanel = this.optionsContainer.GetNode<Panel>("%MusicPanel");
        this.musicLabel = this.optionsContainer.GetNode<RichTextLabel>("%MusicLevelLabel");
        this.musicLabel.Text = (AudioManager.instance.musicPlayer.volume * 100).ToString("N0") + "%";
        this.musicSlider = this.musicPanel.GetNode<HSlider>("%MusicSlider");
        this.musicSlider.Value = AudioManager.instance.musicPlayer.volume * 100;
        this.musicSlider.ValueChanged += (double value) =>
        {
            AudioManager.instance.musicPlayer.SetVolume(value/100);
            this.musicLabel.Text = value.ToString("N0") + "%";
            this.saveButton.button.Disabled = false;
        };
        
        this.sfxPanel = this.optionsContainer.GetNode<Panel>("%SFXPanel");
        this.sfxLabel = this.optionsContainer.GetNode<RichTextLabel>("%SFXLevelLabel");
        this.sfxLabel.Text = (AudioManager.instance.sfxPlayer.volume * 100).ToString("N0") + "%";
        this.sfxSlider = this.sfxPanel.GetNode<HSlider>("%SFXSlider");
        this.sfxSlider.Value = AudioManager.instance.sfxPlayer.volume * 100;
        this.sfxSlider.ValueChanged += (double value) =>
        {
            AudioManager.instance.sfxPlayer.SetVolume(value/100);
            this.sfxLabel.Text = value.ToString("N0") + "%";
            this.saveButton.button.Disabled = false;
        };
        this.windowModeButton = this.optionsContainer.GetNode<MenuButton>("%WindowModeButton");
        this.windowModeButton.GetPopup().IdPressed += OnMenuButtonPressed;
        
        this.resetFTUEButton = this.optionsContainer.GetNode<UIButton>("%ResetFTUEButton");
        this.resetFTUEButton.SetData("Reset");
        this.resetFTUEButton.button.Pressed += OnResetFTUEButtonPressed;


        this.saveButton = this.GetNode<UIButton>("%SaveButton");
        this.saveButton.SetData("Save");
        this.saveButton.button.Pressed += () =>
        {
            GameManager.instance.SaveSettings();
            this.saveButton.button.Disabled = true;
        };
        this.saveButton.button.Disabled = true;
        this.closeButton = this.GetNode<UIButton>("%CloseButton");
        this.closeButton.SetData("Close");
        this.closeButton.button.Pressed += () =>
        {
            UIManager.instance.popUpModel.ClosePopup(UIModel.ViewID.OPTIONS);
        };
    }

    public void OnMenuButtonPressed(long id)
    {
        this.saveButton.button.Disabled = false;
        switch (id)
        {
            case 0:
                this.windowModeButton.Text = "Fullscreen";
                break;
            
            case 1:
                this.windowModeButton.Text = "Windowed";
                break;
            
            case 2:
                this.windowModeButton.Text = "Windowed Fullscreen";
                break;
            
            default:
                break;
        }
        
        GameManager.instance.SetWindowMode(id);
    }

    public void OnResetFTUEButtonPressed()
    {
        UIManager.instance.popUpModel.OpenGenericPopUp(new GenericPopUpView.GenericPopUpData()
        {
            action = () =>
            {
                GameManager.instance.SetFTUE(true);
                UIManager.instance.popUpModel.ClosePopup(UIModel.ViewID.POP_UP, true);
            },
            body = "Are you sure you want to reset the tutorial?",
            buttonText = "Yes",
            title = "Reset Tutorial?"
        });
    }

    public void Enter()
    {
        
    }

    public override void Exit()
    {
        GameManager.instance.LoadSettings();
        this.musicSlider.Value = AudioManager.instance.musicPlayer.volume * 100;
        this.musicLabel.Text = (AudioManager.instance.musicPlayer.volume * 100).ToString("N0") + "%";
        
        this.sfxSlider.Value = AudioManager.instance.sfxPlayer.volume * 100;
        this.sfxLabel.Text = (AudioManager.instance.sfxPlayer.volume * 100).ToString("N0") + "%";
        
        this.saveButton.button.Disabled = true;
        
        base.Exit();
    }
}
