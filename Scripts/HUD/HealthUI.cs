using Godot;
using System;

public partial class HealthUI : Node
{
    private TextureRect healthTexture = null;
    private ProgressBar healthBar = null;
    private RichTextLabel healthLabel = null;
    
    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        this.healthTexture = this.GetNode<TextureRect>("%HealthTexture");
        this.healthBar = this.GetNode<ProgressBar>("%HealthBar");
        this.healthLabel = this.GetNode<RichTextLabel>("%HealthLabel");
    }
    
    public void SetData(PlayerData playerData, bool hideImage = false)
    {
        this.healthBar.Value = (playerData.health/ (float)playerData.maxHealth) * 100f;
        this.healthLabel.Text = playerData.health + " / " + playerData.maxHealth;

        if (hideImage)
        {
            this.healthTexture.Hide();
        }
    }
    
    public void SetData(EnemyData data, bool hideImage = false)
    {
        this.healthBar.Value = (data.health/ (float)data.maxHealth) * 100f;
        this.healthLabel.Text = data.health + " / " + data.maxHealth;

        if (hideImage)
        {
            this.healthTexture.Hide();
        }
    }
}
