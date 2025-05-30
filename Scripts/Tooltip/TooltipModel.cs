using Godot;
using System;

public partial class TooltipModel : UIModel
{
    private RichTextLabel titleLabel = null;
    private RichTextLabel descLabel = null;
    private TextureRect toolTipRect = null;
    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        this.titleLabel = GetNode<RichTextLabel>("TitleLabel");
        this.descLabel = GetNode<RichTextLabel>("DescLabel");
        this.toolTipRect = GetNode<TextureRect>("TooltipRect");
    }

    public void OpenTooltip(PowerUp powerUp)
    {
        this.titleLabel.Text = powerUp.name;
        this.descLabel.Text = powerUp.description;
        this.toolTipRect.Texture = powerUp.texture;
        this.ShowModel();
    }
    
    public void OpenTooltip(Status status)
    {
        this.titleLabel.Text = status.name;
        this.descLabel.Text = status.desc;
        this.toolTipRect.Texture = status.statusIcon;
        this.ShowModel();
    }

    public void CloseTooltip()
    {
        this.HideModel(0.01f);
        this.titleLabel.Text = "";
        this.descLabel.Text = "";
        this.toolTipRect.Texture = ResourceManager.instance.debugIcon;
    }
}
