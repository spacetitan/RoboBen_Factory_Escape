using Godot;
using System;

public partial class TreasureView : UIView
{
    private TextureRect treasureTexture = null;
    private Control playerSpawn = null;
    
    [Export] private Texture2D treasure = null;
    [Export] private Texture2D treasureGlow = null;
    [Export] private Texture2D treasureOpen = null;
    
    private bool isOpened = false;
    public Vector2 playerSize { get; private set; } = Vector2.Zero;
    
    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        this.treasureTexture = this.GetNode<TextureRect>("%TreasureTexture");
        this.playerSpawn = this.GetNode<Control>("PlayerSpawn");
        this.playerSize = new Vector2(this.playerSpawn.Size.X, this.playerSpawn.Size.X);
    }

    public void SetData()
    {
        this.isOpened = false;
        this.treasureTexture.Texture = this.treasure;
        
        Player player = ResourceManager.instance.playerScene.Instantiate() as Player;
        player.GetSceneNodes();
        this.playerSpawn.AddChild(player);
        player.SetCustomMinimumSize(this.playerSize);
        player.SetPlayerData(RunManager.instance.currentRun);
    }

    public override void Enter()
    {
        
    }
    
    public override void Exit()
    {
        this.isOpened = false;
        this.treasureTexture.Texture = this.treasure;
        
        if (this.playerSpawn.GetChildren().Count > 0)
        {
            Player player = this.playerSpawn.GetChild(0) as Player;
            player.DestroyPlayer();
        }
    }

    public void OnGuiInput(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed("LeftMouse") && this.isOpened == false)
        {
            this.isOpened = true;
            this.treasureTexture.Texture = this.treasureOpen;
            
            UIManager.instance.popUpModel.OpenRewardDraft(RewardDraftView.Type.POWERUP, false);
        }
    }

    public void OnMouseEntered()
    {
        if (!this.isOpened)
        {
            this.treasureTexture.Texture = this.treasureGlow;
        }
    }

    public void OnMouseExited()
    {
        if (!this.isOpened)
        {
            this.treasureTexture.Texture = this.treasure;
        }
    }
}
