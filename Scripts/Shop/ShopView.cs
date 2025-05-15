using Godot;
using System;

public partial class ShopView : UIView
{
    private Control playerSpawn = null;
    
    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        this.playerSpawn = this.GetNode<Control>("PlayerSpawn");
    }

    public override void Enter()
    {
        Player player = ResourceManager.instance.playerScene.Instantiate() as Player;
        player.GetSceneNodes();
        this.playerSpawn.AddChild(player);
        player.SetPlayerData(RunManager.instance.currentRun);
    }
    
    public void Exit()
    {
        if (this.playerSpawn.GetChildren().Count > 0)
        {
            Player player = this.playerSpawn.GetChild(0) as Player;
            player.DestroyPlayer();
        }
    }
}
