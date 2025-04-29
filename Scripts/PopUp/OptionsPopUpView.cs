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
    
    private Button saveButton = null;
    private Button closeButton = null;
    
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
        };
        
        
        this.saveButton = this.GetNode<Button>("%SaveButton");
        this.saveButton.Pressed += () =>
        {
            GameManager.instance.SaveSettings();
        };
        this.closeButton = this.GetNode<Button>("%CloseButton");
        this.closeButton.Pressed += () =>
        {
            UIManager.instance.popUpModel.ClosePopup("options");
        };
    }
    
    public override void Exit()
    {
        GameManager.instance.LoadSettings();
        this.musicSlider.Value = AudioManager.instance.musicPlayer.volume * 100;
        this.musicLabel.Text = (AudioManager.instance.musicPlayer.volume * 100).ToString("N0") + "%";
        
        this.sfxSlider.Value = AudioManager.instance.sfxPlayer.volume * 100;
        this.sfxLabel.Text = (AudioManager.instance.sfxPlayer.volume * 100).ToString("N0") + "%";
        
        base.Exit();
    }
}
