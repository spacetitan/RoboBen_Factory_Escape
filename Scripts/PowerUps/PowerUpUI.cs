using Godot;
using System;

public partial class PowerUpUI : Control
{
    private TextureRect powerUpTexture = null;
    private Panel labelPanel = null;
    private RichTextLabel powerUpLabel = null;
    private AnimationPlayer powerUpAP = null;
    
    private PowerUpInfo powerUpInfo = null;
    private PowerUpInfo powerUpInfoInv = null;
    
    public PowerUp powerUp = null;
    public bool isInv = false;

    public void GetSceneNodes()
    {
        this.powerUpTexture = this.GetNode<TextureRect>("%PowerUpTexture");
        this.labelPanel = this.GetNode<Panel>("%LabelPanel");
        this.powerUpLabel = this.labelPanel.GetNode<RichTextLabel>("%PowerUpLabel");
        this.powerUpAP = this.GetNode<AnimationPlayer>("%PowerUpAP");
        this.powerUpInfo = this.GetNode<PowerUpInfo>("%PowerUpInfoPanel");
        this.powerUpInfo.GetSceneNodes();
        this.powerUpInfo.HideView();
        this.powerUpInfoInv = this.GetNode<PowerUpInfo>("%PowerUpInfoPanelInv");
        this.powerUpInfoInv.GetSceneNodes();
        this.powerUpInfoInv.HideView();
    }

    public void SetData(PowerUp powerUp, bool inv = false)
    {
        this.powerUpTexture.Texture = powerUp.texture;
        this.isInv = inv;
        if (powerUp.Value > 0)
        {
            this.labelPanel.Show();
            this.powerUpLabel.Text = powerUp.Value.ToString();
        }
        
        powerUpInfo.SetData(powerUp);
        powerUpInfoInv.SetData(powerUp);
    }
    
    public void Flash()
    {
        this.powerUpAP.Play("flash");
    }
    
    void OnMouseEntered()
    {
        if (this.isInv)
        {
            this.powerUpInfoInv.ShowView();
        }
        else
        {
            this.powerUpInfo.ShowView();
        }
    }

    void OnMouseExited()
    {
        this.powerUpInfo.HideView();
        this.powerUpInfoInv.HideView();
    }
}
