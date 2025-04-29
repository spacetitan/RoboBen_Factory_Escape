using Godot;
using System;

public partial class TitleView : UIView
{
    private Button playButton = null;
    private Button collectionsButton = null;
    private Button optionsButton = null;
    private Button quitButton = null;
    
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
        
        this.playButton = buttonContainer.GetNode<Button>("%PlayButton");
        this.playButton.Pressed += () =>
        {
            this.owner.views["play"].ShowView();
        };
        
        this.collectionsButton = buttonContainer.GetNode<Button>("%CollectionsButton");
        this.collectionsButton.Pressed += () =>
        {
            
        };
        
        this.optionsButton = buttonContainer.GetNode<Button>("%OptionsButton");
        this.optionsButton.Pressed += () =>
        {
            UIManager.instance.popUpModel.OpenPopUp("options");
        };
        
        this.quitButton = buttonContainer.GetNode<Button>("%QuitButton");
        this.quitButton.Pressed += () =>
        {
            this.GetTree().Quit();
        };
    }
}
