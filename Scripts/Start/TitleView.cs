using Godot;
using System;

public partial class TitleView : UIView
{
    private UIButton playButton = null;
    private UIButton collectionsButton = null;
    private UIButton optionsButton = null;
    private UIButton quitButton = null;
    
    public override void InitializeView(UIModel owner)
    {
        this.owner = owner;
    }
    
    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        VBoxContainer buttonContainer = this.GetNode<Panel>("%ButtonPanel").GetNode<VBoxContainer>("%ButtonContainer");
        
        this.playButton = buttonContainer.GetNode<UIButton>("%PlayButton");
        this.playButton.SetData("Play");
        this.playButton.button.Pressed += () =>
        {
            this.owner.views["play"].ShowView();
        };
        
        this.collectionsButton = buttonContainer.GetNode<UIButton>("%CollectionsButton");
        this.collectionsButton.SetData("Collections");
        this.collectionsButton.button.Pressed += () =>
        {
            
        };
        
        this.optionsButton = buttonContainer.GetNode<UIButton>("%OptionsButton");
        this.optionsButton.SetData("Options");
        this.optionsButton.button.Pressed += () =>
        {
            UIManager.instance.popUpModel.OpenPopUp("options");
        };
        
        this.quitButton = buttonContainer.GetNode<UIButton>("%QuitButton");
        this.quitButton.SetData("Quit");
        this.quitButton.button.Pressed += () =>
        {
            this.GetTree().Quit();
        };
    }
}
