using Godot;
using System;

public partial class HealthUI : Node
{
    private ProgressBar healthBar = null;
    private RichTextLabel healthLabel = null;
    
    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        this.healthBar = this.GetNode<ProgressBar>("%HealthBar");
        this.healthLabel = this.GetNode<RichTextLabel>("%HealthLabel");
    }
    
    public void SetData(PlayerData playerData)
    {
        this.healthBar.Value = (playerData.health/ playerData.maxHealth) * 100;
        this.healthLabel.Text = playerData.health + " / " + playerData.maxHealth;
    }
}
