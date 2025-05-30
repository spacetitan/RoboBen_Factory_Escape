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
        this.powerUp = powerUp;
        this.powerUpTexture.Texture = this.powerUp.texture;
        this.powerUpTexture.SetPivotOffset(new Vector2(this.powerUpTexture.Size.X/2 , this.powerUpTexture.Size.Y/2));
        this.isInv = inv;
        if (this.powerUp.showValue)
        {
            this.labelPanel.Show();
            this.powerUpLabel.Text = this.powerUp.value.ToString();
        }
        
        powerUpInfo.SetData(this.powerUp);
        powerUpInfoInv.SetData(this.powerUp);
    }
    
    public void Flash()
    {
        this.powerUpAP.Play("flash");
    }
    
    void OnMouseEntered()
    {
        UIManager.instance.tooltipModel.OpenTooltip(this.powerUp);
        // if (this.isInv)
        // {
        //     this.powerUpInfoInv.ShowView();
        // }
        // else
        // {
        //     this.powerUpInfo.ShowView();
        // }
    }

    void OnMouseExited()
    {
        UIManager.instance.tooltipModel.CloseTooltip();
        // this.powerUpInfo.HideView();
        // this.powerUpInfoInv.HideView();
    }
}
