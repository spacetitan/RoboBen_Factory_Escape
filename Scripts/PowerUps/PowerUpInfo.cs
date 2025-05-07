using Godot;
using System;

public partial class PowerUpInfo : UIView
{
    public RichTextLabel titleLabel = null;
    public RichTextLabel descLabel = null;

    public override void _Ready()
    {
        GetSceneNodes();
    }

    public void GetSceneNodes()
    {
        this.titleLabel = GetNode<RichTextLabel>("%TitleLabel");
        this.descLabel = GetNode<RichTextLabel>("%DescLabel");
    }

    public void SetData(PowerUp data)
    {
        this.titleLabel.Text = data.name;
        this.descLabel.Text = data.description;
    }
}
