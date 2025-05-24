using Godot;
using System;

public partial class HealthUI : Node
{
    private StyleBox healthBarFill = ResourceLoader.Load<StyleBox>("res://Themes/HUD/HealthFill.tres");
    private StyleBox healthBarArmor = ResourceLoader.Load<StyleBox>("res://Themes/HUD/HealthArmor.tres");
    
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

    public void SetData(PlayerData playerData)
    {
        this.healthBar.Value = (playerData.health/ (float)playerData.maxHealth) * 100f;
        this.healthLabel.Text = playerData.health + " / " + playerData.maxHealth;
    }

    public void SetData(Player playerData, Texture2D texture = null, bool hideImage = false)
    {
        this.healthBar.Value = (playerData.playerData.health/ (float)playerData.playerData.maxHealth) * 100f;
        this.healthLabel.Text = playerData.playerData.health + " / " + playerData.playerData.maxHealth;
        
        if (playerData.armor > 0)
        {
            this.healthBar.Value = 100;
            this.healthLabel.Text = playerData.armor.ToString();
            this.healthBar.AddThemeStyleboxOverride("fill", this.healthBarArmor);
        }
        else
        {
            this.healthBar.AddThemeStyleboxOverride("fill", this.healthBarFill);
        }

        if (texture != null)
        {
            this.healthTexture.Texture = texture;
        }
        else
        {
            this.healthTexture.Texture = ResourceManager.instance.HUDIcons[ResourceManager.HUDIconID.HEALTH];
        }

        if (hideImage)
        {
            this.healthTexture.Hide();
        }
        else
        {
            this.healthTexture.Show();
        }
    }
    
    public void SetData(Enemy data, Texture2D texture = null, bool hideImage = false)
    {
        this.healthBar.Value = (data.data.health/ (float)data.data.maxHealth) * 100f;
        this.healthLabel.Text = data.data.health + " / " + data.data.maxHealth;

        if (data.armor > 0)
        {
            this.healthBar.Value = 100;
            this.healthLabel.Text = data.armor.ToString();
            this.healthBar.AddThemeStyleboxOverride("fill", this.healthBarArmor);
        }
        else
        {
            this.healthBar.AddThemeStyleboxOverride("fill", this.healthBarFill);
        }

        if (texture != null)
        {
            this.healthTexture.Texture = texture;
        }
        else
        {
            this.healthTexture.Texture = ResourceManager.instance.HUDIcons[ResourceManager.HUDIconID.HEALTH];
        }

        if (hideImage)
        {
            this.healthTexture.Hide();
        }
        else
        {
            this.healthTexture.Show();
        }
    }
}
